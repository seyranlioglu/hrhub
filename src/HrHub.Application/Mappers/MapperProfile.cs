using HrHub.Abstraction.Contracts.Dtos.ContentTypes;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Mapper;
using HrHub.Domain.Entities.SqlDbEntities;

namespace HrHub.Application.Mappers
{
    public class MapperProfile : MapperProfileBase
    {
        public MapperProfile()
        {
            #region ContentType
            CreateMap<ContentTypeDto, ContentType>().ReverseMap();
            #endregion
            #region Training
            CreateMap<GetTrainingDto, Training>().ReverseMap();
            #endregion
        }
    }
}
