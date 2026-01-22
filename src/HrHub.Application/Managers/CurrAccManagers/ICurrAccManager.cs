using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.CurrAccManagers
{
    public interface ICurrAccManager : IBaseManager
    {
        Task<Response<List<CommonTypeGetDto>>> GetCurrAccRecs(string filterData);
    }
}
