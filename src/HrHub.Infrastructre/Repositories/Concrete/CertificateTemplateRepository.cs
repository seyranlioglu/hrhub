using HrHub.Core.Data.Repository;
using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Infrastructre.Repositories.Concrete
{
    public class CertificateTemplateRepository : EntityRepository<CertificateTemplate>, ICertificateTemplateRepository
    {
        public CertificateTemplateRepository(HrHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
