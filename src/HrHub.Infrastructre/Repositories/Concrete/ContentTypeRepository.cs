using HrHub.Core.Data.Repository;
using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Infrastructre.Repositories.Concrete
{
    public class ContentTypeRepository : EntityRepository<ContentType>, IContentTypeRepository
    {
        public ContentTypeRepository(HrHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
