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

namespace HrHub.Application.Managers.ExamOperationManagers
{
    public class UserExamManager : ManagerBase, IUserExamManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<UserExam> userExamRepository;
        private readonly Repository<UserAnswer> userAnsverRepository;
        public UserExamManager(IHttpContextAccessor httpContextAccessor,
                               IHrUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            userExamRepository = unitOfWork.CreateRepository<UserExam>();
            userAnsverRepository = unitOfWork.CreateRepository<UserAnswer>();
        }


    }
}
