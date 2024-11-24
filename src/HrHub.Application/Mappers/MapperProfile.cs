using HrHub.Abstraction.Contracts.Dtos.ContentTypes;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Mapper;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Domain.Contracts.Dtos.ExamDtos;

namespace HrHub.Application.Mappers
{
    public class MapperProfile : MapperProfileBase
    {
        public MapperProfile()
        {
            #region ContentType
            CreateMap<ContentTypeDto, ContentType>().ReverseMap();
            CreateMap<Exam, AddExamDto>().ReverseMap();
            CreateMap<ExamTopic, AddExamTopicDto>().ReverseMap();
            CreateMap<AddExamQuestionDto, ExamQuestion>()
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.QuestionOptions))
                .ReverseMap();
            #endregion
            #region Training
            CreateMap<GetTrainingDto, Training>().ReverseMap();
            #endregion
        }
    }
}
