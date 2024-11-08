using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingContent : TypeCardEntity<int>
    {
        public TrainingContent()
        {
            ContentComments = new HashSet<ContentComment>();
        }
        public int SectionId { get; set; }
        public string ContentType { get; set; }
        public int Time { get; set; }
        public int PageCount { get; set; }
        public float CompletedRate { get; set; }
        public string FilePath { get; set; }
        public bool Mandatory { get; set; }
        public int OrderId { get; set; }
        public bool AllowSeeking { get; set; }
        public int PartCount { get; set; }
        public int MinReadTimeThreshold { get; set; }

        public virtual ICollection<ContentComment> ContentComments { get; set; } = null;
    }

}
