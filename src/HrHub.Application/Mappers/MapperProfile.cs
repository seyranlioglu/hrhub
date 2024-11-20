using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Core.Mapper;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Domain.Contracts.Dtos.ExamDtos;

namespace HrHub.Application.Mappers
{
    public class MapperProfile : MapperProfileBase
    {
        public MapperProfile()
        {
            CreateMap<ContentTypeDto, ContentType>().ReverseMap();
            CreateMap<Exam, AddExamDto>().ReverseMap();
            CreateMap<ExamTopic, AddExamTopicDto>().ReverseMap();
        }
    }
}
