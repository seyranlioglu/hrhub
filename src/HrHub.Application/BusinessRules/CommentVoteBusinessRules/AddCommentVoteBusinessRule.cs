using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CommentVoteDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.BusinessRules.CommentVoteBusinessRules
{
    public class AddCommentVoteBusinessRule : IAddCommentVoteBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<CommentVote>commentVoteRepository;
        private readonly Repository<ContentComment> contentCommentRepository;
        private readonly Repository<User> userRepository;


        public AddCommentVoteBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.commentVoteRepository = hrUnitOfWork.CreateRepository<CommentVote>();
            this.contentCommentRepository = hrUnitOfWork.CreateRepository<ContentComment>(); 
            this.userRepository = hrUnitOfWork.CreateRepository<User>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is AddCommentVoteDto contentCommentDto && contentCommentDto is not null)
            {
                var isContentCommentExist = commentVoteRepository
                   .Exists(
                   predicate: p => p.UserId == contentCommentDto.UserId
                                   && p.ContentCommentId == contentCommentDto.ContentCommentId);
                if (isContentCommentExist)
                    return (false, ValidationMessages.CommentVoteExistsError);

                var trainingId = contentCommentRepository.Get(p => p.Id == contentCommentDto.ContentCommentId, include: i => i.Include(a => a.TrainingContent).ThenInclude(a=>a.TrainingSection), selector: s => s).TrainingContent.TrainingSection.TrainingId;
                var currAccTrainingUsers = userRepository.Get(p => p.Id == contentCommentDto.UserId, include: i => i.Include(a => a.CurrAcc).ThenInclude(b => b.CurrAccTrainings).ThenInclude(o => o.CurrAccTrainingUsers), selector: s => s);
                if (!currAccTrainingUsers.CurrAccTrainings.Any(s => s.TrainingId == trainingId))
                    return (false, ValidationMessages.ContentCommenNotTrainingUserError);
            }

            return (true, string.Empty);
        }

    }
}
