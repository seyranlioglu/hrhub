using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingContent : TypeCardEntity<long>
    {
        public TrainingContent()
        {
            ContentComments = new HashSet<ContentComment>();
            ContentNotes = new HashSet<ContentNote>();
            UserContentsViewLogs = new HashSet<UserContentsViewLog>();
        }
        public long TrainingSectionId { get; set; }
        public long ContentTypeId { get; set; }
        public TimeSpan Time { get; set; }
        [AllowNull]
        public int? PageCount { get; set; }
        [AllowNull]
        public decimal CompletedRate { get; set; }
        [AllowNull]
        public string FilePath { get; set; }
        public bool Mandatory { get; set; }
        public long OrderId { get; set; }
        [AllowNull]
        public bool? AllowSeeking { get; set; }
        public int PartCount { get; set; }
        [AllowNull]
        public int? MinReadTimeThreshold { get; set; }
        [AllowNull]
        public long? ExamId { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }
        [ForeignKey("SectionId")]
        public virtual TrainingSection TrainingSection { get; set; } = null;

        [ForeignKey("ContentTypeId")]
        public virtual ContentType ContentType { get; set; } = null;

        public virtual ICollection<ContentComment> ContentComments { get; set; } = null;
        public virtual ICollection<ContentNote> ContentNotes { get; set; } = null;
        public virtual ICollection<UserContentsViewLog> UserContentsViewLogs { get; set; } = null;
    }

}
