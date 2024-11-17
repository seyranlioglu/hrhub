using HrHub.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamResult : CardEntity<long>
    {
        public long ExamId { get; set; }
        public int VersionNumber { get; set; }
        public long UserId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal Score { get; set; }
        public DateTime ResultDate { get; set; }
        public bool QualifiedCertificate { get; set; }
        public long CertificateTypeId { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("CertificateTypeId")]
        public virtual CertificateType CertificateType { get; set; }

    }
}
