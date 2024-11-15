using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Data.UnitOfWork;
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
    public class ExamManager : ManagerBase, IExamManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Exam> examRepository;
        public ExamManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            examRepository = unitOfWork.CreateRepository<Exam>();
        }

        public void Deneme()
        {
            var result = examRepository.Get(w => w.IsDelete == false);
        }
    }
}
