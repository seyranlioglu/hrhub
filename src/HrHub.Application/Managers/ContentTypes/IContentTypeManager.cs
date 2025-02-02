using HrHub.Domain.Contracts.Dtos.ContentTypes;
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
        Task<Response<CommonResponse>> AddContentTypeAsync(AddContentTypeDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteContentTypeAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<ContentTypeDto>> GetByIdForContentTypeAsync(long id);
        Task<Response<IEnumerable<ContentTypeDto>>> GetListForContentTypeAsync();
        Task<Response<CommonResponse>> UpdateContentTypeAsync(UpdateContentTypeDto data, CancellationToken cancellationToken = default);
    }
}
