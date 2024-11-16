using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingSection : TypeCardEntity<long>
    {
        public TrainingSection()
        {
            TrainingContents = new HashSet<TrainingContent>();
        }
        public long TrainingId { get; set; }
        public long RowNumber { get; set; }
        public string? LangCode { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        public virtual ICollection<TrainingContent> TrainingContents { get; set; }
    }

}
