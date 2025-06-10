using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Core.Data.Repository;
using HrHub.Core.Utilties.Encryption;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.BusinessRules.UserBusinessRules
{
    public class UserBusinessRule : IUserBusinessRule
    {
        private readonly Repository<PasswordHistory> passwordHistoryRepository;
        private readonly Repository<User> userRepository;

        public UserBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.passwordHistoryRepository = unitOfWork.CreateRepository<PasswordHistory>();
            this.userRepository = unitOfWork.CreateRepository<User>();
        }
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is UserSignUpDto userSignUpDto && userSignUpDto is not null)
            {
                var isContentTypeExist = userRepository
                   .GetList(
                   predicate: p => p.UserName == userSignUpDto.Email
                                   || p.PhoneNumber == userSignUpDto.PhoneNumber);
                if (isContentTypeExist.Any())
                    return (false, ValidationMessages.UserMailOrPhoneNumberAlReadyExists);

            }

            if (value is AddUserDto addUserDto && addUserDto is not null)
            {
                var isContentTypeExist = userRepository
                   .GetList(
                   predicate: p => p.UserName == addUserDto.Email
                                   || p.PhoneNumber == addUserDto.PhoneNumber);
                if (isContentTypeExist.Any())
                    return (false, ValidationMessages.UserMailOrPhoneNumberAlReadyExists);

            }
            if (value is UserSignInDto userSignInDto && userSignInDto is not null)
            {
                var user = userRepository
                  .Get(
                  predicate: p => p.UserName == userSignInDto.UserName);
                if (user == null)
                    return (false, ValidationMessages.UserNotFound);
                if (!user.EmailConfirmed)
                {
                    return (false, ValidationMessages.UserMailNotConfirmed);
                }
                if (!user.PhoneNumberConfirmed)
                {
                    return (false, ValidationMessages.UserPhoneNotConfirmed);
                }
            }
            if (value is ChangePasswordDto changePasswordDto && changePasswordDto is not null)
            {
                var user = userRepository
                  .Get(
                  predicate: p => p.UserName == changePasswordDto.UserName);
                if (user == null)
                    return (false, ValidationMessages.UserNotFound);
                if (!user.EmailConfirmed)
                {
                    return (false, ValidationMessages.UserMailNotConfirmed);
                }
                if (!user.PhoneNumberConfirmed)
                {
                    return (false, ValidationMessages.UserPhoneNotConfirmed);
                }
            }
            if (value is ForgotPasswordDto forgotPasswordDto && forgotPasswordDto is not null)
            {
                var user = userRepository
                  .Get(
                  predicate: p => p.UserName == forgotPasswordDto.UserName);
                if (user == null)
                    return (false, ValidationMessages.UserNotFound);
                if (!user.EmailConfirmed)
                {
                    return (false, ValidationMessages.UserMailNotConfirmed);
                }
                if (!user.PhoneNumberConfirmed)
                {
                    return (false, ValidationMessages.UserPhoneNotConfirmed);
                }
            }
            
            if (value is PasswordResetDto passwordResetDto && passwordResetDto is not null)
            {
                var user = userRepository
                  .Get(
                  predicate: p => p.UserName == passwordResetDto.UserName);
                if (user == null)
                    return (false, ValidationMessages.UserNotFound);
                var userPasswords =  passwordHistoryRepository
               .GetPagedList(predicate: ph => ph.UserId == user.Id,
                                  orderBy: o => o.OrderByDescending(p => p.CreatedDate),
                                  skip: 0, take: 3,
                                  selector: s => AesEncrypion.DecryptString(s.Password));

                bool isPasswordUsed = userPasswords.Any(decryptedPassword => decryptedPassword == passwordResetDto.Password);
                if (isPasswordUsed)
                    return (false, ValidationMessages.SameAsLastThreePasswords); 
            }
            
            return (true, string.Empty);
        }
    }
}
