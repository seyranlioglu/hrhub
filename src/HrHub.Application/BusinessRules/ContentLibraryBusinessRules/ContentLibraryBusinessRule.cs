using AutoMapper;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.ContentLibraryBusinessRules
{
    public class ContentLibraryBusinessRule : IContentLibraryBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<ContentLibrary> contentLibraryRepository;

        public ContentLibraryBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.contentLibraryRepository = hrUnitOfWork.CreateRepository<ContentLibrary>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            return (false, string.Empty); 
        }
    }
}
