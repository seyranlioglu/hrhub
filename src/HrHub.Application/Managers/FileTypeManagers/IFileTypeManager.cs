using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.FileTypeDtos;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;

namespace HrHub.Application.Managers.FileTypeManagers
{
    public interface IFileTypeManager : IBaseManager
    {
        Task<Response<GetFileTypeDto>> GetByIdFileTypeAsync(string fileExtension);
    }
}
