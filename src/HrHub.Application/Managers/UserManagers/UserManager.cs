using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.UserBusinessRules;
using HrHub.Application.Factories;
using HrHub.Application.Helpers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Core.HrFluentValidation;
using HrHub.Core.Utilties.Encryption;
using HrHub.Domain.Contracts.Dtos.NotificationDtos;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Identity.Model;
using HrHub.Identity.Services;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System.Linq.Expressions;

namespace HrHub.Application.Managers.UserManagers
{
    public class UserManager : ManagerBase, IUserManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<CurrAcc> currAccRepository;
        private readonly Repository<User> userRepository;
        private readonly Repository<Instructor> instructorRepository;
        private readonly IAppUserService appUserService;
        private readonly IAppRoleService appRoleService;
        private readonly IAuthenticationService authenticationService;
        private readonly Repository<PasswordHistory> passwordHistoryRepository;
        private readonly MessageSenderFactory messageSenderFactory;
        public UserManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork unitOfWork, IMapper mapper, IAppUserService appUserService, IAppRoleService appRoleService, IAuthenticationService authenticationService, MessageSenderFactory messageSenderFactory) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currAccRepository = unitOfWork.CreateRepository<CurrAcc>();
            this.instructorRepository = unitOfWork.CreateRepository<Instructor>();
            this.userRepository = unitOfWork.CreateRepository<User>();
            this.appUserService = appUserService;
            this.appRoleService = appRoleService;
            this.authenticationService = authenticationService;
            this.passwordHistoryRepository = unitOfWork.CreateRepository<PasswordHistory>(); ;
            this.messageSenderFactory = messageSenderFactory;
        }
        public async Task<Response<UserSignUpResponse>> SignUp(UserSignUpDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UserSignUpDto>(data, typeof(IUserBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<UserSignUpResponse>();

            var currAcc = mapper.Map<CurrAcc>(data);
            currAcc.CreatedDate = DateTime.UtcNow;

            var curAccEntity = await currAccRepository.AddAndReturnAsync(currAcc);
            await unitOfWork.SaveChangesAsync();
            string password = PasswordHepler.GeneratePassword(8, true, true, true);
            var signUpModel = mapper.Map<SignUpDto>(data);
            signUpModel.Password = password;
            signUpModel.AuthCode = Guid.NewGuid().TrimHyphen();
            signUpModel.CurrAccId = curAccEntity.Id;
            signUpModel.IsMainUser = true;
            var result = await appUserService.SignUpAsync(signUpModel);
            if (result.Item2)
            {

                var appUser = await appUserService.GetUserByEmailAsync(data.Email);
                if (appUser != null)
                {
                    var roles = await appRoleService.GetRoleList();
                    if (roles != null)
                    {
                        var userRole = roles.FirstOrDefault(s => s.Name == Roles.User);
                        if (userRole != null)
                        {
                            await appUserService.AddUserRole(appUser, new Identity.Entities.AppRole { Id = userRole.Id, Name = userRole.Name });
                            var passwordHistory = new PasswordHistory
                            {
                                UserId = appUser.Id,
                                PasswordChangeDate = DateTime.UtcNow,
                                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                                ChangeReason = "Password created",
                                Password = AesEncrypion.EncryptString(password),
                                CreatedDate = DateTime.UtcNow
                            };
                            await passwordHistoryRepository.AddAsync(passwordHistory);
                            await unitOfWork.SaveChangesAsync();
                            return ProduceSuccessResponse(new UserSignUpResponse
                            {
                                Result = true,
                                Message = "Kullanıcı Kaydedildi.",
                                Email = data.Email,
                                PhoneNumber = data.PhoneNumber
                            });
                        }
                    }
                }
            }

            return ProduceFailResponse<UserSignUpResponse>("Kullanıcı Kaydedilemedi.", StatusCodes.Status500InternalServerError);

        }
        public async Task<Response<VerifySendResponse>> VerifyCodeSend(VerifySendDto verifySendDto, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<VerifySendDto>();
            var validateResult = validator.Validate(verifySendDto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<VerifySendResponse>();

            string message = "";
            if (verifySendDto.Type == SubmissionTypeEnum.Sms)
            {
                message = VerificationHelper.MaskPhoneNumber(verifySendDto.Receiver) + "  telefonunuza doğrulama kodu gönderilmiştir.";
            }
            else
            {
                message = VerificationHelper.MaskEmail(verifySendDto.Receiver) + " mail adresinize doğrulama kodu gönderilmiştir.";
            }
            // Doğrulama kodu oluştur ve gönder
            string verificationCode = VerificationHelper.GenerateVerificationCode();
            await SendVerifyCode(verifySendDto.Receiver, verificationCode, verifySendDto.Type, MessageTemplates.Register);
            VerificationHelper.SaveCode("Confirm_" + verifySendDto.Receiver, verificationCode);
            return ProduceSuccessResponse(new VerifySendResponse { Result = true, Message = message });
        }
        public async Task<Response<VerifyResponse>> VerifyCodeAndConfirm(VerifyDto verify, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<VerifyDto>();
            var validateResult = validator.Validate(verify);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<VerifyResponse>();


            string? storedCode = VerificationHelper.GetCode("Confirm_" + verify.CodeParameter);
            if (storedCode == null || storedCode != verify.Code)
                return ProduceFailResponse<VerifyResponse>("Doğrulama kodu geçersiz!", StatusCodes.Status401Unauthorized);
            var confirmResult = await ConfirmUser(verify.UserName, verify.SubmissionType);
            if (!confirmResult.Result)
                return ProduceFailResponse<VerifyResponse>(confirmResult.Message, confirmResult.Code);
            return ProduceSuccessResponse(new VerifyResponse { Result = true, Message = "Doğrulama Başarılı." });
        }
        public async Task<Response<VerifySignInResponse>> VerifyCodeAndSignIn(VerifySignInDto verify, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<VerifySignInDto>();
            var validateResult = validator.Validate(verify);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<VerifySignInResponse>();


            string? storedCode = VerificationHelper.GetCode("SignIn_" + verify.UserName);
            if (storedCode == null || storedCode != verify.Code)
                return ProduceFailResponse<VerifySignInResponse>("Doğrulama kodu geçersiz!", StatusCodes.Status401Unauthorized);

            //TODO: cache servisi düzelnce açılacak
            //  var cacheToken = await cacheService.GetAsync<VerifySignInResponse>("SignIn_" + verify.UserName, cancellationToken);
            // await cacheService.RemoveAsync<VerifySignInResponse>("SignIn_" + verify.UserName);

            string? responseString = VerificationHelper.GetCode("SignInResponse_" + verify.UserName);
            return ProduceSuccessResponse(Newtonsoft.Json.JsonConvert.DeserializeObject<VerifySignInResponse>(responseString));
        }

        public async Task<CommonResponse> ConfirmUser(string userName, SubmissionTypeEnum submissionType)
        {
            if (submissionType == SubmissionTypeEnum.Email)
            {
                var user = await appUserService.GetUserByEmailAsync(userName);
                if (user == null)
                    return new CommonResponse { Message = "Kullanıcı Bulunamadı", Code = StatusCodes.Status404NotFound, Result = false };

                var token = await appUserService.GenerateEmailConfirmationTokenAsync(user);
                if (token == null)
                    return new CommonResponse { Message = "Email Onay Anahtarı Oluşturulamadı", Code = StatusCodes.Status404NotFound, Result = false };
                var confirmResult = await appUserService.ConfirmEmailAsync(user, token);
                if (!confirmResult)
                    return new CommonResponse { Message = "Mail doğrulama başarısız", Code = StatusCodes.Status500InternalServerError, Result = false };
                return new CommonResponse { Result = true, Message = "Mail Doğrulama Başarılı.", Code = StatusCodes.Status200OK };
            }
            else
            {
                var user = await userRepository.GetAsync(P => P.Email == userName);
                if (user == null)
                    return new CommonResponse { Message = "Kullanıcı Bulunamadı", Code = StatusCodes.Status404NotFound, Result = false };
                user.PhoneNumberConfirmed = true;
                userRepository.Update(user);
                await unitOfWork.SaveChangesAsync();


                //Telefon doğrulama sonrası kullanıcı ve parole mail olarak gönderiliyor
                var userLastPassword = await passwordHistoryRepository
              .GetPagedListAsync(predicate: ph => ph.UserId == user.Id,
                                 orderBy: o => o.OrderByDescending(p => p.CreatedDate),
                                 skip: 0, take: 1,
                                 selector: s => AesEncrypion.DecryptString(s.Password));

                string content = MailHelper.GetMailBody(MailType.AddUser);
                var dictionary = new Dictionary<string, string>
                {
                    { "@PASSWORD", userLastPassword.First() },
                    { "@USERNAME", user.Email }
                };
                var sender = messageSenderFactory.GetSender(MessageType.Email);
                await sender.SendAsync(new EmailMessageDto { Recipient = user.Email, Content = content, MessageTemplate = MessageTemplates.NewUser, Parameters = dictionary });


                return new CommonResponse { Result = true, Message = "Telefon Doğrulama Başarılı.", Code = StatusCodes.Status200OK };
            }
        }

        public async Task<Response<UserSignInResponse>> SignIn(UserSignInDto request, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UserSignInDto>(request, typeof(IUserBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<UserSignInResponse>();

            var user = await userRepository.GetAsync(p => p.Email == request.UserName, include: i => i.Include(s => s.Instructor));
            var result = await authenticationService.SignIn(new SignInViewModelResource { Email = request.UserName, Password = request.Password });
            if (result == null)
                return ProduceFailResponse<UserSignInResponse>("Kullanıcı Girişi Başarısız..", StatusCodes.Status500InternalServerError);
            VerifySignInResponse response = new VerifySignInResponse
            {

                Expiration = result.Expiration,
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                CurrAccId = user.CurrAccId,
                Id = user.Id,
                Name = user.Name,
                SurName = user.SurName,
                UserShortName = user.UserShortName,
                PhoneNumber = user.PhoneNumber,
                InstructorCode = user.Instructor?.InstructorCode
            };
            string receiver = request.UserName;
            string message = VerificationHelper.MaskEmail(request.UserName) + " mail adresinize doğrulama kodu gönderilmiştir.";
            SubmissionTypeEnum type = SubmissionTypeEnum.Email;
            if (request.Type == SubmissionTypeEnum.Sms)
            {

                receiver = user.PhoneNumber;
                message = VerificationHelper.MaskPhoneNumber(receiver) + "  telefonunuza doğrulama kodu gönderilmiştir.";
                type = SubmissionTypeEnum.Sms;
            }

            // Doğrulama kodu oluştur ve gönder
            string verificationCode = VerificationHelper.GenerateVerificationCode();
            await SendVerifyCode(receiver, verificationCode, type, MessageTemplates.Login);
            VerificationHelper.SaveCode("SignIn_" + request.UserName, verificationCode);
            VerificationHelper.SaveCode("SignInResponse_" + request.UserName, Newtonsoft.Json.JsonConvert.SerializeObject(response));
            // await cacheService.SetAsync(response, "SignIn_" + request.UserName, cancellationToken);
            return ProduceSuccessResponse(new UserSignInResponse { Result = true, Message = message });
        }

        public async Task<Response<CommonResponse>> AddUser(AddUserDto request, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<AddUserDto>(request, typeof(IUserBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();
            string password = PasswordHepler.GeneratePassword(8, true, true, true);
            var signUpModel = mapper.Map<SignUpDto>(request);
            signUpModel.AuthCode = Guid.NewGuid().TrimHyphen();
            signUpModel.Password = password;
            signUpModel.IsMainUser = false;

            var result = await appUserService.SignUpAsync(signUpModel);
            if (result.Item2)
            {
                var appUser = await appUserService.GetUserByEmailAsync(request.Email);
                if (appUser != null)
                {
                    var roles = await appRoleService.GetRoleList();
                    if (roles != null)
                    {
                        var userRole = roles.FirstOrDefault(s => s.Name == Roles.User);
                        if (userRole != null)
                        {
                            await appUserService.AddUserRole(appUser, new Identity.Entities.AppRole { Id = userRole.Id, Name = userRole.Name });

                            var passwordHistory = new PasswordHistory
                            {
                                UserId = appUser.Id,
                                PasswordChangeDate = DateTime.UtcNow,
                                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                                ChangeReason = "Password created",
                                Password = AesEncrypion.EncryptString(password),
                                CreatedDate = DateTime.UtcNow
                            };
                            await passwordHistoryRepository.AddAsync(passwordHistory);
                            await unitOfWork.SaveChangesAsync();

                            //kullanıcı ve parola bilgilieri kullanıcıya mail olarak gönderilecek
                            string content = MailHelper.GetMailBody(MailType.AddUser);
                            var dictionary = new Dictionary<string, string>
                            {
                                { "@PASSWORD", password },
                                { "@USERNAME", request.Email }
                            };
                            var sender = messageSenderFactory.GetSender(MessageType.Email);
                            await sender.SendAsync(new EmailMessageDto { Recipient = request.Email, Content = content, MessageTemplate = MessageTemplates.NewUser, Parameters = dictionary });
                        }
                    }
                }
            }

            return ProduceFailResponse<CommonResponse>("Kullanıcı Kaydedilemedi.", StatusCodes.Status500InternalServerError);
        }

        public async Task<Response<CommonResponse>> ChangePassword(ChangePasswordDto changePassword, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<ChangePasswordDto>(changePassword, typeof(IUserBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();
            var user = await appUserService.GetUserByEmailAsync(changePassword.UserName);
            string receiver = changePassword.UserName;
            string message = VerificationHelper.MaskEmail(changePassword.UserName) + " mail adresinize şifre değişikliği için doğrulama kodu gönderilmiştir.";
            SubmissionTypeEnum type = SubmissionTypeEnum.Email;
            if (changePassword.Type == SubmissionTypeEnum.Sms)
            {

                receiver = user.PhoneNumber;
                message = VerificationHelper.MaskPhoneNumber(receiver) + "  telefonunuza şifre değişikliği için doğrulama kodu gönderilmiştir.";
                type = SubmissionTypeEnum.Sms;
            }

            // Doğrulama kodu oluştur ve gönder
            string verificationCode = VerificationHelper.GenerateVerificationCode();
            await SendVerifyCode(receiver, verificationCode, type, MessageTemplates.ChangePassword);
            VerificationHelper.SaveCode("ChangePassword_" + changePassword.UserName, verificationCode);
            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = message });
        }
        public async Task<Response<CommonResponse>> VerifyCodeAndChangePassword(VerifyChangePasswordDto verify, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<VerifyChangePasswordDto>();
            var validateResult = validator.Validate(verify);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();


            string? storedCode = VerificationHelper.GetCode("ChangePassword_" + verify.UserName);
            if (storedCode == null || storedCode != verify.Code)
                return ProduceFailResponse<CommonResponse>("Doğrulama kodu geçersiz!", StatusCodes.Status401Unauthorized);

            return ProduceSuccessResponse(new CommonResponse { Message = "Doğrulama Başarılı", Code = 200, Result = true });
        }

        public async Task<Response<CommonResponse>> ForgotPassword(ForgotPasswordDto forgotPassword, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<ForgotPasswordDto>(forgotPassword, typeof(IUserBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();


            var user = await appUserService.GetUserByEmailAsync(forgotPassword.UserName);
            string receiver = forgotPassword.UserName;
            string message = VerificationHelper.MaskEmail(forgotPassword.UserName) + " mail adresinize şifre değişikliği için doğrulama kodu gönderilmiştir.";
            SubmissionTypeEnum type = SubmissionTypeEnum.Email;
            if (forgotPassword.Type == SubmissionTypeEnum.Sms)
            {

                receiver = user.PhoneNumber;
                message = VerificationHelper.MaskPhoneNumber(receiver) + "  telefonunuza şifre değişikliği için doğrulama kodu gönderilmiştir.";
                type = SubmissionTypeEnum.Sms;
            }

            // Doğrulama kodu oluştur ve gönder
            string verificationCode = VerificationHelper.GenerateVerificationCode();
            await SendVerifyCode(receiver, verificationCode, type, MessageTemplates.ChangePassword);
            VerificationHelper.SaveCode("ForgotPassword_" + forgotPassword.UserName, verificationCode);
            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = message });
        }
        public async Task<Response<CommonResponse>> VerifyCodeAndForgotPassword(VerifyForgotPasswordDto verify, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<VerifyForgotPasswordDto>();
            var validateResult = validator.Validate(verify);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();


            string? storedCode = VerificationHelper.GetCode("ForgotPassword_" + verify.UserName);
            if (storedCode == null || storedCode != verify.Code)
                return ProduceFailResponse<CommonResponse>("Doğrulama kodu geçersiz!", StatusCodes.Status401Unauthorized);

            return ProduceSuccessResponse(new CommonResponse { Message = "Doğrulama Başarılı", Code = 200, Result = true });
        }


        public async Task<Response<CommonResponse>> PasswordReset(PasswordResetDto passwordReset, string reason, bool isSendMail = false, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<PasswordResetDto>();
            var validateResult = validator.Validate(passwordReset);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            var user = await appUserService.GetUserByEmailAsync(passwordReset.UserName);
            var token = await authenticationService.GeneratePasswordResetTokenAsync(user);
            var newPasswordResult = await authenticationService.ResetPasswordAsync(user, token, passwordReset.Password);
            if (!newPasswordResult)
                return ProduceFailResponse<CommonResponse>("Şifre değiştirme başarısız.!", StatusCodes.Status500InternalServerError);

            var passwordHistory = new PasswordHistory
            {
                UserId = user.Id,
                PasswordChangeDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                ChangeReason = reason,
                Password = AesEncrypion.EncryptString(passwordReset.Password),
                CreatedDate = DateTime.UtcNow
            };
            await passwordHistoryRepository.AddAsync(passwordHistory);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            if (isSendMail)
            {
                // Süper Admin değişiklik yapıyorsa, password değişen kişiye yeni bilgilierinin Mail gönderme işlemleri yapılıyor

                string content = MailHelper.GetMailBody(MailType.ChangePasswordBySuperAdmin);
                var dictionary = new Dictionary<string, string>
                {
                    { "@PASSWORD", passwordReset.Password },
                    { "@USERNAME", user.Email }
                };
                var sender = messageSenderFactory.GetSender(MessageType.Email);
                await sender.SendAsync(new EmailMessageDto { Recipient = user.Email, Content = content, MessageTemplate = MessageTemplates.ChangePassword, Parameters = dictionary });

            }
            return ProduceSuccessResponse(new CommonResponse { Message = "Şifre başarıyla değiştirilmiştir.", Code = 200, Result = true });
        }
        public async Task<Response<GetUserResponse>> GetUserById(GetUserByIdDto getUserById, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<GetUserByIdDto>();
            var validateResult = validator.Validate(getUserById);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<GetUserResponse>();
            var user = await userRepository.GetAsync(p => p.Id == getUserById.Id, selector: s => mapper.Map<GetUserResponse>(s));
            if (user == null)
                return ProduceFailResponse<GetUserResponse>("Kullanıcı Bulunamadı", StatusCodes.Status404NotFound);

            return ProduceSuccessResponse(user);
        }

        public async Task<Response<List<GetUserResponse>>> GetUserList(CancellationToken cancellationToken = default)
        {
            List<Attributes> list = new List<Attributes>();
            if (!this.IsSuperAdmin() && this.IsMainUser())
            {
                list.Add(new Attributes { Name = "CurrAccId", Value = this.GetCurrAccId(), Type = ExpressionType.Equal });
            }
            var predicate = FilterHelper<User>.GeneratePredicate(list);
            var user = await userRepository.GetListAsync(predicate: predicate, selector: s => mapper.Map<GetUserResponse>(s));
            return ProduceSuccessResponse(user.ToList());
        }

        public async Task<Response<CommonResponse>> SetUserStatus(SetUserStatusDto setUserStatusDto, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<SetUserStatusDto>();
            var validateResult = validator.Validate(setUserStatusDto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();
            var user = await userRepository.GetAsync(p => p.Id == setUserStatusDto.UserId);
            if (user == null)
                return ProduceFailResponse<CommonResponse>("Kullanıcı Bulunamadı", StatusCodes.Status404NotFound);

            user.IsActive = setUserStatusDto.IsActive;
            user.UpdateDate = DateTime.UtcNow;
            user.UpdateUserId = this.GetCurrentUserId();
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Message = "Kullanıcı durumu başarıyla değiştirilmiştir.", Code = 200, Result = true });

        }

        public async Task<Response<CommonResponse>> UpdateUser(UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<UserUpdateDto>();
            var validateResult = validator.Validate(userUpdateDto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();
            var user = await userRepository.GetAsync(p => p.Id == userUpdateDto.Id);
            if (user == null)
                return ProduceFailResponse<CommonResponse>("Kullanıcı Bulunamadı", StatusCodes.Status404NotFound);

            user.Name = userUpdateDto.Name;
            user.SurName = userUpdateDto.SurName;
            user.CurrAccId = userUpdateDto.CurrAccId;
            user.UpdateDate = DateTime.UtcNow;
            user.UpdateUserId = this.GetCurrentUserId();
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Message = "Kullanıcı başarıyla güncellenmiştir.", Code = 200, Result = true });
        }
        public async Task<Response<CommonResponse>> DeleteUser(long userId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetAsync(p => p.Id == userId);
            if (user == null)
                return ProduceFailResponse<CommonResponse>("Kullanıcı Bulunamadı", StatusCodes.Status404NotFound);

            user.DeleteDate = DateTime.UtcNow;
            user.DeleteUserId = this.GetCurrentUserId();
            user.IsActive = false;
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Message = "Kullanıcı başarıyla silinmiştir.", Code = 200, Result = true });
        }
        public async Task SendVerifyCode(string receiver, string code, SubmissionTypeEnum type, MessageTemplates template)
        {
            switch (type)
            {

                case SubmissionTypeEnum.Email:

                    var content = MailHelper.GetMailBody(MailType.VerifyEmail);
                    var dictionary = new Dictionary<string, string>
                {
                    { "@VERIFYCODE", code }
                };
                    var sender = messageSenderFactory.GetSender(MessageType.Email);
                    await sender.SendAsync(new EmailMessageDto { Recipient = receiver, Content = content, MessageTemplate = template, Parameters = dictionary });
                    break;
                case SubmissionTypeEnum.Sms:
                    var senderSms = messageSenderFactory.GetSender(MessageType.Sms);
                    await senderSms.SendAsync(new SmsMessageDto { Recipient = receiver, Content = "Doğrulama Kodu : " + code, MessageTemplate = template, Parameters = new Dictionary<string, string>() });
                    break;
            }
        }

        public async Task<Response<CommonResponse>> SetUserInstructor(UserInstructorDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<UserInstructorDto>();
            var validateResult = validator.Validate(dto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            var user = await userRepository.GetAsync(p => p.Id == dto.UserId, include: i => i.Include(c => c.CurrAcc));
            if (user == null)
                return ProduceFailResponse<CommonResponse>("Kullanıcı Bulunamadı", StatusCodes.Status404NotFound);
            var exist = await instructorRepository.ExistsAsync(p => p.UserId == dto.UserId);
            if (exist)
                return ProduceFailResponse<CommonResponse>("Kullanıcı Zaten Instructor", StatusCodes.Status409Conflict);
            var entity = mapper.Map<Instructor>(dto);
            entity.Address = user.CurrAcc.Address;
            entity.Phone = user.PhoneNumber;
            entity.Email = user.Email;
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.PicturePath = "test";
            entity.InstructorCode = "INS_" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await instructorRepository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Message = "İşlem Tamamlandı.", Code = 200, Result = true });
        }
    }
}
