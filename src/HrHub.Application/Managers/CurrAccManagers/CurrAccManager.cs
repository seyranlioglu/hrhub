using HrHub.Abstraction.Result;
using HrHub.Application.Helpers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CommonDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.CurrAccManagers
{
    public class CurrAccManager : ManagerBase, ICurrAccManager
    {
        private readonly Repository<CurrAcc> currAccRepository;
        private readonly IHrUnitOfWork hrUnitOfWork;
        public CurrAccManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork hrUnitOfWork) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            currAccRepository = hrUnitOfWork.CreateRepository<CurrAcc>();
        }

        public async Task<Response<List<CommonTypeGetDto>>> GetCurrAccRecs(string filterData)
        {
            System.Linq.Expressions.Expression<Func<CurrAcc, bool>> predicate = x => x.IsDelete != true && x.IsActive == true;

            if (!string.IsNullOrEmpty(filterData))
            {
                predicate = predicate.And(x => x.Title.ToLower().Contains(filterData.ToLower()));
            }

            var result = await currAccRepository.GetPagedListAsync(
                predicate: predicate,
                skip: 0,
                take: 10,
                selector : x => new CommonTypeGetDto
                {
                    Id = x.Id,
                    Code = x.Name,
                    Title = x.Title
                }
            );

            return ProduceSuccessResponse(result.ToList());
        }
    }
}
