using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.ContentCommentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.BusinessRules.ContentCommentBusinessRules
{
    public class AddContentCommentBusinessRule : IAddContentCommentBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<ContentComment> contentCommentRepository;
        private readonly Repository<TrainingContent> trainingContentRepository;
        private readonly Repository<User> userRepository;


        public AddContentCommentBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.contentCommentRepository = hrUnitOfWork.CreateRepository<ContentComment>();
            this.trainingContentRepository = hrUnitOfWork.CreateRepository<TrainingContent>(); 
            this.userRepository = hrUnitOfWork.CreateRepository<User>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is AddContentCommentDto contentCommentDto && contentCommentDto is not null)
            {
                var isContentCommentExist = contentCommentRepository
                   .Exists(
                   predicate: p => p.UserId == contentCommentDto.UserId
                                   && p.ContentId == contentCommentDto.ContentId);
                if (isContentCommentExist)
                    return (false, ValidationMessages.ContentCommenExistsError);

                var trainingId = trainingContentRepository.Get(p => p.Id == contentCommentDto.ContentId, include: i => i.Include(a => a.TrainingSection), selector: s => s).TrainingSection.TrainingId;
                var currAccTrainingUsers = userRepository.Get(p => p.Id == contentCommentDto.UserId, include: i => i.Include(a => a.CurrAcc).ThenInclude(b => b.CurrAccTrainings).ThenInclude(o => o.CurrAccTrainingUsers), selector: s => s);
                if (!currAccTrainingUsers.CurrAccTrainings.Any(s => s.TrainingId == trainingId))
                    return (false, ValidationMessages.ContentCommenNotTrainingUserError);
            }

            return (true, string.Empty);
        }

    }
}
