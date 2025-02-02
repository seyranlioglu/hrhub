using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentLibrary : TypeCardEntity<long>
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long? FileTypeId { get; set; }
        public long? TrainingContentId { get; set; }
        public string? Thumbnail { get; set; }

        [ForeignKey("TrainingContentId")]
        public virtual TrainingContent TrainingContent { get; set; }

        [ForeignKey("FileTypeId")]
        public virtual FileType FileType { get; set; }
    }
}
