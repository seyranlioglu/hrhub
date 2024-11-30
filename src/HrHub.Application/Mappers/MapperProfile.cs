using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Mapper;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Entities.SqlDbEntities;

namespace HrHub.Application.Mappers
{
    public class MapperProfile : MapperProfileBase
    {
        public MapperProfile()
        {
            #region ContentType
            CreateMap<ContentTypeDto, ContentType>().ReverseMap();
            CreateMap<AddExamDto, Exam>()
            .ForMember(dest => dest.ExamVersions, opt => opt.MapFrom(src => new List<AddExamVersionDto> { src.VersionInfo }));
            CreateMap<ExamTopic, AddExamTopicDto>().ReverseMap();
            CreateMap<AddExamQuestionDto, ExamQuestion>()
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.QuestionOptions))
                .ReverseMap();
            #endregion
            #region Training

            CreateMap<AddTrainingDto, Training>().ReverseMap();
            CreateMap<UpdateTrainingDto, Training>().ReverseMap();
            CreateMap<DeleteTrainingDto, Training>().ReverseMap();
            CreateMap<Training, GetTrainingDto>()
           .ForMember(dest => dest.CategoryCode, opt => opt.MapFrom(src => src.TrainingCategories.First().Code))
           .ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.TrainingCategories.First().Title))
           .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.TrainingCategories.First().Description))

           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Instructors.First().Email))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Instructors.First().Title))

           .ForMember(dest => dest.CompletionTimeUnitCode, opt => opt.MapFrom(src => src.TimeUnits.First().Code))
           .ForMember(dest => dest.CompletionTimeUnitDescription, opt => opt.MapFrom(src => src.TimeUnits.First().Description))

           .ForMember(dest => dest.TrainingLevelCode, opt => opt.MapFrom(src => src.TrainingLevels.First().Code))
           .ForMember(dest => dest.TrainingLevelDescription, opt => opt.MapFrom(src => src.TrainingLevels.First().Description))

           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.HeaderImage, opt => opt.MapFrom(src => src.HeaderImage))
           .ForMember(dest => dest.LangCode, opt => opt.MapFrom(src => src.LangCode))
           .ForMember(dest => dest.TrainingType, opt => opt.MapFrom(src => src.TrainingType))
           .ForMember(dest => dest.CurrentAmount, opt => opt.MapFrom(src => src.CurrentAmount))
           .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
           .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DiscountRate))
           .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => src.TaxRate))
           .ForMember(dest => dest.CertificateOfAchievementRate, opt => opt.MapFrom(src => src.CertificateOfAchievementRate))
           .ForMember(dest => dest.CertificateOfParticipationRate, opt => opt.MapFrom(src => src.CertificateOfParticipationRate))
           .ForMember(dest => dest.CompletionTime, opt => opt.MapFrom(src => src.CompletionTime));



            #endregion

            #region
            CreateMap<AddWhatYouWillLearnDto, WhatYouWillLearn>().ReverseMap();
            CreateMap<UpdateWhatYouWillLearnDto, WhatYouWillLearn>().ReverseMap();
            CreateMap<DeleteWhatYouWillLearnDto, WhatYouWillLearn>().ReverseMap();
            #endregion

            #region TrainingSection
            CreateMap<AddTrainingSectionDto, TrainingSection>().ReverseMap();
            CreateMap<UpdateTrainingSectionDto, TrainingSection>().ReverseMap();
            CreateMap<DeleteTrainingSectionDto, TrainingSection>().ReverseMap();
            CreateMap<GetTrainingSectionDto, TrainingSection>().ReverseMap();
            #endregion

            #region TrainingContent
            CreateMap<AddTrainingContentDto, TrainingContent>().ReverseMap();
            CreateMap<UpdateTrainingContentDto, TrainingContent>().ReverseMap();
            CreateMap<DeleteTrainingContentDto, TrainingContent>().ReverseMap();
            CreateMap<GetTrainingContentDto, TrainingContent>().ReverseMap();
            #endregion
        }
    }
}
