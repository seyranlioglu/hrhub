using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.MenuDtos;

public interface IMenuManager : IBaseManager
{
    Task<Response<UserMenuResponseDto>> GetCurrentUserMenuAsync();
}