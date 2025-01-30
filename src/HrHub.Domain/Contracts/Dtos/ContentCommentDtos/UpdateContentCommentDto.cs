using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.ContentCommentDtos
{
    public class UpdateContentCommentDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long Id { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public string Title { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public string Description { get; set; }
        public bool Pinned { get; set; }
        public long? MasterContentId { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public int StarCount { get; set; }
    }
}
