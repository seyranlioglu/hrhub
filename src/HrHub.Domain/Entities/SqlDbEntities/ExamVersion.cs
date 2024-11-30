using HrHub.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
        [AllowNull]
        public string? VersionDescription { get; set; }
        public long ExamId { get; set; }
        public int VersionNumber { get; set; }
        [DefaultValue(false)]
        public bool IsPublished { get; set; }
        [AllowNull]
        public TimeSpan? ExamTime { get; set; }
        [AllowNull]
        public decimal? SuccessRate { get; set; }
        [AllowNull]
        public decimal? PassingScore { get; set; }
        /// <summary>
        /// Sınavda görüntülenecek toplam soru sayısını belirtiyor.
        /// </summary>
        [AllowNull]
        public int? TotalQuestionCount { get; set; }
        public long ExamVersionStatusId { get; set; }

        [ForeignKey("ExamVersionStatusId")]
        public virtual ExamVersionStatus ExamVersionStatus { get; set; }
        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
        public virtual ICollection<ExamTopic> ExamTopics { get; set; } = null;

        public virtual ICollection<UserExam> UserExams { get; set; } = null;

    }
}
