using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.BusinessRules.TrainingAnnouncementBusinessRules
{
    public class AddTrainingAnnouncementBusinessRule : IAddTrainingAnnouncementBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingAnnouncement> trainingAnnouncementRepository;
        private readonly Repository<ContentComment> contentCommentRepository;
        private readonly Repository<User> userRepository;


        public AddTrainingAnnouncementBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingAnnouncementRepository = hrUnitOfWork.CreateRepository<TrainingAnnouncement>();
            this.contentCommentRepository = hrUnitOfWork.CreateRepository<ContentComment>(); 
            this.userRepository = hrUnitOfWork.CreateRepository<User>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is AddTrainingAnnouncementDto addTrainingAnnouncementDto && addTrainingAnnouncementDto is not null)
            {
                var isContentCommentExist = trainingAnnouncementRepository
                   .Exists(
                   predicate: p => p.UserId == addTrainingAnnouncementDto.UserId
                                   && p.TrainingId == addTrainingAnnouncementDto.TrainingId);
                if (isContentCommentExist)
                    return (false, ValidationMessages.TrainingAnnouncementExistsError);

                var trainingId = addTrainingAnnouncementDto.TrainingId;
                var currAccTrainingUsers = userRepository.Get(p => p.Id == addTrainingAnnouncementDto.UserId, include: i => i.Include(a => a.CurrAcc).ThenInclude(b => b.CurrAccTrainings).ThenInclude(o => o.CurrAccTrainingUsers), selector: s => s);
                if (!currAccTrainingUsers.CurrAccTrainings.Any(s => s.TrainingId == trainingId))
                    return (false, ValidationMessages.TrainingAnnouncementNotTrainingUserError);
            }

            return (true, string.Empty);
        }

    }
}
