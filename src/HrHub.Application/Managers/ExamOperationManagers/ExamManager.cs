using HrHub.Abstraction.Contracts.Dtos.CommonDtos;
using HrHub.Application.Managers.TypeManagers;
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
        private readonly ICommonTypeBaseManager<CertificateType> certificateManager;
        public ExamManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork unitOfWork, ICommonTypeBaseManager<CertificateType> certificateManager) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.certificateManager = certificateManager;
        }

        public async Task Deneme()
        {
            var certType = await certificateManager.GetByIdAsync<CommonTypeGetDto>(2);
        }
    }
}
