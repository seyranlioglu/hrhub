using AutoMapper;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.TypeManagers
{
    public class CurrAccTypeManager : CommonTypeBaseManager<CurrAccType>, ICurrAccTypeManager
    {
        public CurrAccTypeManager(
            IMapper mapper,
            IHrUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IRepository<CurrAccType> repository)
            : base(httpContextAccessor, unitOfWork)
        {
        }
    }
}
