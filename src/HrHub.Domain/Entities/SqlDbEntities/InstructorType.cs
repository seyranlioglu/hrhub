using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class InstructorType : TypeCardEntity<int>
    {
        public string LangCode { get; set; }
    }
}
