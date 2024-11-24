using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.Users
{
    public class UserManager : ManagerBase, IUserManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<User> userRepository;
        public UserManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork hrUnitOfWork) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            userRepository = hrUnitOfWork.CreateRepository<User>();
        }

        public async Task<bool> IsMainUser()
        {
            bool isMainUser = await userRepository.ExistsAsync(user => user.Id == this.GetCurrentUserId());
            return isMainUser;
        }
    }
}
