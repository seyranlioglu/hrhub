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
            CreateMap<AddExamDto, Exam>()
            .ForMember(dest => dest.ExamVersions, opt => opt.MapFrom(src => new List<AddExamVersionDto> { src.VersionInfo }));
            CreateMap<ExamTopic, AddExamTopicDto>().ReverseMap();
            CreateMap<AddExamQuestionDto, ExamQuestion>()
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.QuestionOptions))
                .ReverseMap();
        }
    }
}
