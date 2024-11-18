using HrHub.Abstraction.Contracts.Dtos.ContentTypes;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.ContentTypes
{
    public interface IContentTypeManager : IBaseManager
    {
        Task<Response<ContentTypeDto>> GetByIdForContentTypeAsync(long id);
        Task<Response<IEnumerable<ContentTypeDto>>> GetListForContentTypeAsync();
    }
}
