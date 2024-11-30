using HrHub.Abstraction.Result;

namespace HrHub.Application.Managers.Users
{
    public interface IUserManager
    {
        Task<bool> IsMainUser();
    }
}
