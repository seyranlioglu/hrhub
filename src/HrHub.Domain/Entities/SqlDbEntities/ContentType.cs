using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentType : TypeCardEntity<long>
    {
        public ContentType()
        {
            TrainingContents = new HashSet<TrainingContent>();
        }
        public string LangCode { get; set; }


        public virtual ICollection<TrainingContent> TrainingContents { get; set; }
    }
}
