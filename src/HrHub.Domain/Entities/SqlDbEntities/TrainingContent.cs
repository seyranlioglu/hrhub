using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingContent : TypeCardEntity<long>
    {
        public TrainingContent()
        {
            ContentComments = new HashSet<ContentComment>();
            ContentNotes = new HashSet<ContentNote>();
            UserContentsViewLogs = new HashSet<UserContentsViewLog>();
            ContentExams = new HashSet<ContentExam>();
        }
        public long TrainingSectionId { get; set; }
        public long ContentTypeId { get; set; }
        public TimeSpan Time { get; set; }
        public int PageCount { get; set; }
        public decimal CompletedRate { get; set; }
        public string FilePath { get; set; }
        public bool Mandatory { get; set; }
        public long OrderId { get; set; }
        public bool AllowSeeking { get; set; }
        public int PartCount { get; set; }
        public int MinReadTimeThreshold { get; set; }


        [ForeignKey("SectionId")]
        public virtual TrainingSection TrainingSection { get; set; } = null;

        [ForeignKey("ContentTypeId")]
        public virtual ContentType ContentType { get; set; } = null;

        public virtual ICollection<ContentComment> ContentComments { get; set; } = null;
        public virtual ICollection<ContentNote> ContentNotes { get; set; } = null;
        public virtual ICollection<UserContentsViewLog> UserContentsViewLogs { get; set; } = null;
        public virtual ICollection<ContentExam> ContentExams { get; set; } = null;
    }

}
