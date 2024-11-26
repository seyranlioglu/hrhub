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
            CreateMap<ExamVersion, ExamVersion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.VersionNumber, opt => opt.MapFrom(src => src.VersionNumber + 1))
                .ForMember(dest => dest.ExamTopics, opt => opt.MapFrom(src => src.ExamTopics.Select(t => new ExamTopic
                {
                    Title = t.Title,
                    ImgPath = t.ImgPath,
                    QuestionCount = t.QuestionCount,
                    SeqNumber = t.SeqNumber,
                    ExamQuestions = t.ExamQuestions.Select(q => new ExamQuestion
                    {
                        QuestionText = q.QuestionText,
                        Score = q.Score,
                        QuestionOptions = q.QuestionOptions.Select(o => new QuestionOption
                        {
                            OptionText = o.OptionText,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    }).ToList()
                })));
        }
    }
}
