using AutoMapper;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.Helpers;
using HrHub.Cache.Services;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.HrFluentValidation;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Identity.Model;
using HrHub.Identity.Services;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using ServiceStack;

namespace HrHub.Application.Managers.UserManagers
{
    public class UserManager : ManagerBase, IUserManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<CurrAcc> currAccRepository;
        private readonly Repository<User> userRepository;

        private readonly IAppUserService appUserService;
        private readonly IAppRoleService appRoleService;
     //   private readonly ICacheService cacheService;
        private readonly IAuthenticationService authenticationService;
        public UserManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork unitOfWork, IMapper mapper, IAppUserService appUserService, IAppRoleService appRoleService,/* ICacheService cacheService,*/ IAuthenticationService authenticationService) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currAccRepository = unitOfWork.CreateRepository<CurrAcc>();
            this.userRepository = unitOfWork.CreateRepository<User>();
            this.appUserService = appUserService;
            this.appRoleService = appRoleService;
           // this.cacheService = cacheService;
            this.authenticationService = authenticationService;
        }
        public async Task<bool> IsMainUser()
        {
            bool isMainUser = await userRepository.ExistsAsync(user => user.Id == this.GetCurrentUserId());
            return isMainUser;
        }
        public async Task<Response<UserSignUpResponse>> SignUp(UserSignUpDto data, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<UserSignUpDto>();
            var validateResult = validator.Validate(data);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<UserSignUpResponse>();

            var currAcc = mapper.Map<CurrAcc>(data);
            var curAccEntity = await currAccRepository.AddAndReturnAsync(currAcc);
            string password = PasswordHepler.GeneratePassword(8, true, true, true);
            var signUpModel = mapper.Map<SignUpDto>(data);
            signUpModel.Password = password;
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
            VerificationHelper.SendVerifyCode(verifySendDto.Receiver, verificationCode, verifySendDto.Type);
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
            return ProduceSuccessResponse(new VerifySignInResponse { });
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
                return new CommonResponse { Result = true, Message = "Telefon Doğrulama Başarılı.", Code = StatusCodes.Status200OK };
            }
        }

        public async Task<Response<UserSignInResponse>> SignIn(UserSignInDto request, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<UserSignInDto>();
            var validateResult = validator.Validate(request);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<UserSignInResponse>();

            var user = await appUserService.GetUserByEmailAsync(request.UserName);
            if (!user.EmailConfirmed)
            {
                return ProduceFailResponse<UserSignInResponse>("Kullanıcı mail doğrulama işlemleri henüz tamamlanmamış. Lütfen Önce mail doğrulama işlemlerini tamamlayınız.", StatusCodes.Status500InternalServerError);
            }
            if (!user.PhoneNumberConfirmed)
            {
                return ProduceFailResponse<UserSignInResponse>("Kullanıcı telefon doğrulama işlemleri henüz tamamlanmamış. Lütfen Önce telefon doğrulama işlemlerini tamamlayınız.", StatusCodes.Status500InternalServerError);
            }
            var result = await authenticationService.SignIn(new SignInViewModelResource { Email = request.UserName, Password = request.Password });
            if (result == null)
                return ProduceFailResponse<UserSignInResponse>("Kullanıcı Girişi Başarısız..", StatusCodes.Status500InternalServerError);

            string receiver = request.UserName;
            string message = VerificationHelper.MaskEmail(request.UserName) + " mail adresinize doğrulama kodu gönderilmiştir.";
            SubmissionTypeEnum type = SubmissionTypeEnum.Email;
            if (user.PhoneNumberConfirmed && !user.PhoneNumber.IsNullOrEmpty())
            {

                receiver = user.PhoneNumber;
                message = VerificationHelper.MaskPhoneNumber(receiver) + "  telefonunuza doğrulama kodu gönderilmiştir.";
                type = SubmissionTypeEnum.Sms;
            }

            // Doğrulama kodu oluştur ve gönder
            string verificationCode = VerificationHelper.GenerateVerificationCode();
            VerificationHelper.SendVerifyCode(receiver, verificationCode, type);
            VerificationHelper.SaveCode("SignIn_" + request.UserName, verificationCode);
           // await cacheService.SetAsync(result, "SignIn_" + request.UserName, cancellationToken);
            return ProduceSuccessResponse(new UserSignInResponse { Result = true, Message = message });
        }
    }
}
