using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingContent : TypeCardEntity<long>
    {
        public long SectionId { get; set; }
        public string ContentType { get; set; }
        public long Time { get; set; }
        public long PageCount { get; set; }
        public float CompletedRate { get; set; }
        public string FilePath { get; set; }
        public bool Mandatory { get; set; }
        public long OrderId { get; set; }
        public bool AllowSeeking { get; set; }
        public long PartCount { get; set; }
        public long MinReadTimeThreshold { get; set; }
    }

}
