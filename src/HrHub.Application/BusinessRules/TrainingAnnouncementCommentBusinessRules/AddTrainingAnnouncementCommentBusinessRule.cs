using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.BusinessRules.TrainingAnnouncementCommentBusinessRules
{
    public class AddTrainingAnnouncementCommentBusinessRule : IAddTrainingAnnouncementCommentBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingAnnouncementsComment> trainingAnnouncementCommentRepository;
        private readonly Repository<TrainingAnnouncement> trainingAnnouncementRepository;
        private readonly Repository<User> userRepository;


        public AddTrainingAnnouncementCommentBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingAnnouncementCommentRepository = hrUnitOfWork.CreateRepository<TrainingAnnouncementsComment>();
            this.trainingAnnouncementRepository = hrUnitOfWork.CreateRepository<TrainingAnnouncement>(); 
            this.userRepository = hrUnitOfWork.CreateRepository<User>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is AddTrainingAnnouncementCommentDto addTrainingAnnouncementCommentDto && addTrainingAnnouncementCommentDto is not null)
            {
                var isContentCommentExist = trainingAnnouncementCommentRepository
                   .Exists(
                   predicate: p => p.UserId == addTrainingAnnouncementCommentDto.UserId
                                   && p.TrainingAnnouncementsId == addTrainingAnnouncementCommentDto.TrainingAnnouncementsId);
                if (isContentCommentExist)
                    return (false, ValidationMessages.TrainingAnnouncementCommentExistsError);

                long trainingId = trainingAnnouncementRepository.Get(p => p.Id == addTrainingAnnouncementCommentDto.TrainingAnnouncementsId, selector: s => s).TrainingId;
                var currAccTrainingUsers = userRepository.Get(p => p.Id == addTrainingAnnouncementCommentDto.UserId, include: i => i.Include(a => a.CurrAcc).ThenInclude(b => b.CurrAccTrainings).ThenInclude(o => o.CurrAccTrainingUsers), selector: s => s);
                if (!currAccTrainingUsers.CurrAccTrainings.Any(s => s.TrainingId == trainingId))
                    return (false, ValidationMessages.TrainingAnnouncementCommentNotTrainingUserError);
            }

            return (true, string.Empty);
        }

    }
}
