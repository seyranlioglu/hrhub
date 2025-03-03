using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingAnnouncementCommentBusinessRules
{

    public class UpdateTrainingAnnouncementCommentBusinessRule : IUpdateTrainingAnnouncementCommentBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingAnnouncementsComment> trainingAnnouncementCommentRepository;


        public UpdateTrainingAnnouncementCommentBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingAnnouncementCommentRepository = hrUnitOfWork.CreateRepository<TrainingAnnouncementsComment>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is UpdateTrainingAnnouncementCommentDto trainingAnnouncementCommentDto && trainingAnnouncementCommentDto is not null)
            {
                var isTrainingAnnouncementCommentExist = trainingAnnouncementCommentRepository
                   .Exists(
                   predicate: p => p.Id == trainingAnnouncementCommentDto.Id);
                if (isTrainingAnnouncementCommentExist)
                    return (false, ValidationMessages.TrainingAnnouncementCommentNotFoundError);
                }

            return (true, string.Empty);
        }
    }

}