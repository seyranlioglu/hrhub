using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CertificateType : TypeCardEntity<long>
    {
        public CertificateType()
        {
            CertificateTemplates = new HashSet<CertificateTemplate>();
            ExamResults = new HashSet<ExamResult>();
        }
        public string? LangCode { get; set; }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }


        public virtual ICollection<CertificateTemplate> CertificateTemplates { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; } = null;
    }
}
