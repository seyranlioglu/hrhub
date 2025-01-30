using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.ContentCommentDtos
{
    public class AddContentCommentDto
    {
        [ValidationRules(typeof(NullCheckRule))] 
        public string Title { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string Description { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ContentId { get; set; }
        public bool Pinned { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long UserId { get; set; }
        public long? MasterContentId { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public int StarCount { get; set; }
    }
}
