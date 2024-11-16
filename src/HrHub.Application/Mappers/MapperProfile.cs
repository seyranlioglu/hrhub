using HrHub.Abstraction.Contracts.Dtos.ContentTypes;
using HrHub.Core.Mapper;
using HrHub.Domain.Entities.SqlDbEntities;

namespace HrHub.Application.Mappers
{
    public class MapperProfile : MapperProfileBase
    {
        public MapperProfile()
        {
            CreateMap<ContentTypeDto, ContentType>().ReverseMap();
        }
    }
}
