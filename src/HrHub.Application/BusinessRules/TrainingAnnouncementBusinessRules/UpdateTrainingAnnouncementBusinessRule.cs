using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingAnnouncementBusinessRules
{

    public class UpdateTrainingAnnouncementBusinessRule : IUpdateTrainingAnnouncementBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingAnnouncement> trainingAnnouncementRepository;


        public UpdateTrainingAnnouncementBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingAnnouncementRepository = hrUnitOfWork.CreateRepository<TrainingAnnouncement>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is UpdateTrainingAnnouncementDto trainingAnnouncementDto && trainingAnnouncementDto is not null)
            {
                var isTrainingAnnouncementExist = trainingAnnouncementRepository
                   .Exists(
                   predicate: p => p.Id == trainingAnnouncementDto.Id);
                if (isTrainingAnnouncementExist)
                    return (false, ValidationMessages.TrainingAnnouncementNotFoundError);
                }

            return (true, string.Empty);
        }
    }

}