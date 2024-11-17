using HrHub.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamVersion: CardEntity<long>
    {
        public ExamVersion()
        {
            ExamTopics = new HashSet<ExamTopic>();
            UserExams = new HashSet<UserExam>();
        }
        public long ExamId { get; set; }
        public int VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        /// <summary>
        /// Versiyonun Geçerli olacağı tarih
        /// </summary>
        public DateTime EffectiveFrom { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
        public virtual ICollection<ExamTopic> ExamTopics { get; set; } = null;

        public virtual ICollection<UserExam> UserExams { get; set; } = null;

    }
}
