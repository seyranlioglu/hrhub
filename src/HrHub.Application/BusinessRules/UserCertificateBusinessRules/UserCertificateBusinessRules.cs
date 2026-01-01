using HrHub.Application.BusinessRules.UserCertificateBusinessRules;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CertificateDtos; // DTO eklendi
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.UserCertificateBusinessRules
{
    public class UserCertificateBusinessRule : IUserCertificateBusinessRule
    {
        private readonly IHrUnitOfWork _unitOfWork;
        private readonly Repository<UserCertificate> _userCertificateRepository;

        // NOT: Eğer bu sınıf Servis olarak register edilmiyorsa, 
        // constructor injection çalışmayacaktır. ValidationHelper içinde 
        // ServiceLocator ile çözümlenmesi gerekir. Şimdilik constructor kalıyor.
        public UserCertificateBusinessRule(IHrUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userCertificateRepository = unitOfWork.CreateRepository<UserCertificate>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            // Gelen değerin CheckCertificateEligibilityDto olup olmadığına bakıyoruz
            if (value is CheckCertificateEligibilityDto checkDto)
            {
                var isCertificateExists = _userCertificateRepository.Exists(
                    predicate: x => x.CurrAccTrainingUsersId == checkDto.CurrAccTrainingUsersId && x.IsDelete != true
                );

                if (isCertificateExists)
                {
                    return (false, "Bu eğitim için zaten tanımlı bir sertifika mevcut.");
                }
            }

            return (true, string.Empty);
        }
    }
}