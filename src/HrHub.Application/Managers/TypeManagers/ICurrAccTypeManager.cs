using HrHub.Application.Managers.TypeManagers;
using HrHub.Domain.Contracts.Dtos.UserDtos; // DTO'nun olduğu namespace
using HrHub.Domain.Entities.SqlDbEntities;

namespace HrHub.Application.Managers.TypeManagers
{
    // CommonTypeBaseManager'dan gelen tüm Get, Add, Update yeteneklerini miras alır.
    // TEntity: CurrAccType, TDto: CurrAccTypeDto
    public interface ICurrAccTypeManager : ICommonTypeBaseManager<CurrAccType>
    {
    }
}