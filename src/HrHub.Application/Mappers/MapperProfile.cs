using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Mapper;
using HrHub.Domain.Contracts.Dtos.CertificateDtos;
using HrHub.Domain.Contracts.Dtos.CommentVoteDtos;
using HrHub.Domain.Contracts.Dtos.ContentCommentDtos;
using HrHub.Domain.Contracts.Dtos.ContentLibraryDtos;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Contracts.Dtos.CurrAccTrainingDtos;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Contracts.Dtos.FileTypeDtos;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearnsDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Identity.Model;

namespace HrHub.Application.Mappers
{
    public class MapperProfile : MapperProfileBase
    {
        public MapperProfile()
        {
            CreateMap<UserCertificate, UserCertificateDto>()
                .ForMember(dest => dest.TemplateTitle, opt => opt.MapFrom(src => src.CertificateTemplate.Title))
                .ForMember(dest => dest.TrainingName, opt => opt.MapFrom(src => src.CurrAccTrainingUser.CurrAccTrainings.Training.Name));

            CreateMap<AddUserCertificateDto, UserCertificate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // DB Primary Key (long) ignore
                .ForMember(dest => dest.CreateUserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsDelete, opt => opt.MapFrom(src => false));
            // Not: CertificateId isimleri tuttuğu için otomatik maplenir.

            CreateMap<UserCertificate, AddUserCertificateDto>().ReverseMap();


            #region ContentType
            CreateMap<ContentTypeDto, ContentType>().ReverseMap();
            CreateMap<AddExamDto, Exam>()
            .ForMember(dest => dest.ExamVersions, opt => opt.MapFrom(src => new List<AddExamVersionDto> { src.VersionInfo }));
            CreateMap<AddExamVersionDto, ExamVersion>()
    .ForMember(dest => dest.ExamTime, opt => opt.MapFrom(src =>
        src.ExamTime.HasValue ? src.ExamTime.Value : TimeSpan.Zero));
            CreateMap<ExamTopic, AddExamTopicDto>().ReverseMap();
            CreateMap<AddExamQuestionDto, ExamQuestion>()
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.QuestionOptions))
                .ReverseMap();
            #endregion
            #region Training

            CreateMap<AddTrainingDto, Training>().ReverseMap();
            //        CreateMap<AddTrainingDto, Training>()
            //.ForMember(dest => dest.ForWhomId, opt => opt.MapFrom(src => src.ForWhomId == 0 ? (long?)null : (long?)src.ForWhomId))
            //.ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId == 0 ? (long?)null : (long?)src.InstructorId))
            //.ForMember(dest => dest.CompletionTimeUnitId, opt => opt.MapFrom(src => src.CompletionTimeUnitId == 0 ? (long?)null : (long?)src.CompletionTimeUnitId))
            //.ForMember(dest => dest.TrainingLevelId, opt => opt.MapFrom(src => src.TrainingLevelId == 0 ? (long?)null : (long?)src.TrainingLevelId))
            //.ForMember(dest => dest.PreconditionId, opt => opt.MapFrom(src => src.PreconditionId == 0 ? (long?)null : (long?)src.PreconditionId))
            //.ForMember(dest => dest.EducationLevelId, opt => opt.MapFrom(src => src.EducationLevelId == 0 ? (long?)null : (long?)src.EducationLevelId))
            //.ForMember(dest => dest.PriceTierId, opt => opt.MapFrom(src => src.PriceTierId == 0 ? (long?)null : (long?)src.PriceTierId));

            CreateMap<UpdateTrainingDto, Training>()

                .ForMember(dest => dest.TrainingLanguageId, opt =>
                {        
                    opt.Condition(src => src.TrainingLanguageId != 0);
                    opt.MapFrom(src => src.TrainingLanguageId);
                })
              
               .ForMember(dest => dest.TrainingLanguage, opt => opt.Ignore()) 

               .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
               {
                   if (srcMember == null)
                       return false;

                   if (srcMember is string str)
                       return !string.IsNullOrWhiteSpace(str);


                   if (srcMember is long l)
                       return l != 0;

                   if (srcMember is int i)
                       return i != 0;

                   if (srcMember is decimal d)
                       return d != 0;

                   if (srcMember is DateTime dt)
                       return dt != default;

                   if (srcMember is IEnumerable<object> col && !(srcMember is string))
                       return col.Any();

                   if (srcMember is long valLong)
                   {
                       long? nullableLong = valLong;
                       return nullableLong.HasValue && nullableLong.Value != 0;
                   }

                   if (srcMember is int valInt)
                   {
                       int? nullableInt = valInt;
                       return nullableInt.HasValue && nullableInt.Value != 0;
                   }

                   if (srcMember is decimal valDecimal)
                   {
                       decimal? nullableDecimal = valDecimal;
                       return nullableDecimal.HasValue && nullableDecimal.Value != 0;
                   }

                   if (srcMember is DateTime valDateTime)
                   {
                       DateTime? nullableDateTime = valDateTime;
                       return nullableDateTime.HasValue && nullableDateTime.Value != default;
                   }

                   return true;
               }));



            CreateMap<DeleteTrainingDto, Training>().ReverseMap();

            CreateMap<Training, GetTrainingDto>()
                .ForMember(dest => dest.CategoryCode, opt => opt.MapFrom(src => src.TrainingCategory.Code))
                .ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.TrainingCategory.Title))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.TrainingCategory.Description))

                .ForMember(dest => dest.CompletionTimeUnitCode, opt => opt.MapFrom(src => src.TimeUnit.Code))
                .ForMember(dest => dest.CompletionTimeUnitDescription, opt => opt.MapFrom(src => src.TimeUnit.Description))

                .ForMember(dest => dest.TrainingLevelCode, opt => opt.MapFrom(src => src.TrainingLevel.Code))
                .ForMember(dest => dest.TrainingLevelDescription, opt => opt.MapFrom(src => src.TrainingLevel.Description))

                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HeaderImage, opt => opt.MapFrom(src => src.HeaderImage))
                .ForMember(dest => dest.LangCode, opt => opt.MapFrom(src => src.LangCode))
                .ForMember(dest => dest.CurrentAmount, opt => opt.MapFrom(src => src.CurrentAmount))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DiscountRate))
                .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => src.TaxRate))
                .ForMember(dest => dest.CertificateOfAchievementRate, opt => opt.MapFrom(src => src.CertificateOfAchievementRate))
                .ForMember(dest => dest.CertificateOfParticipationRate, opt => opt.MapFrom(src => src.CertificateOfParticipationRate))
                .ForMember(dest => dest.CompletionTime, opt => opt.MapFrom(src => src.CompletionTime))
                .ForMember(dest => dest.TrainingSections, opt => opt.MapFrom(src => src.TrainingSections))
                .ForMember(dest => dest.WhatYouWillLearns, opt => opt.MapFrom(src => src.WhatYouWillLearns.Where(w => w.TrainingId == src.Id).ToList()));



            CreateMap<WhatYouWillLearn, WhatYouWillLearnDto>()
                   .ForMember(dest => dest.WhatYouWillLearnId, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.WhatYouWillLearnIsDelete, opt => opt.MapFrom(src => src.IsDelete))
                   .ForMember(dest => dest.WhatYouWillLearnIsActive, opt => opt.MapFrom(src => src.IsActive))
                   .ForMember(dest => dest.WhatYouWillLearnTitle, opt => opt.MapFrom(src => src.Title))
                   .ForMember(dest => dest.WhatYouWillLearnAbbreviation, opt => opt.MapFrom(src => src.Abbreviation))
                   .ForMember(dest => dest.WhatYouWillLearnCode, opt => opt.MapFrom(src => src.Code))
                   .ForMember(dest => dest.WhatYouWillLearnDescription, opt => opt.MapFrom(src => src.Description))
                   .ForMember(dest => dest.WhatYouWillLearnTrainingId, opt => opt.MapFrom(src => src.TrainingId));


            CreateMap<TrainingSection, TrainingSectionDto>()
                    .ForMember(dest => dest.TrainingSectionId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.TrainingSectionCode, opt => opt.MapFrom(src => src.Code))
                    .ForMember(dest => dest.TrainingSectionLangCode, opt => opt.MapFrom(src => src.LangCode))
                    .ForMember(dest => dest.TrainingSectionRowNumber, opt => opt.MapFrom(src => src.RowNumber))
                    .ForMember(dest => dest.TrainingSectionDescription, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.TrainingSectionTitle, opt => opt.MapFrom(src => src.Title));

            CreateMap<TrainingContent, TrainingContentDto>()
                                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dest => dest.TrainingSectionId, opt => opt.MapFrom(src => src.TrainingSectionId))
                                .ForMember(dest => dest.ContentTypeId, opt => opt.MapFrom(src => src.ContentTypeId))
                                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
                                .ForMember(dest => dest.PageCount, opt => opt.MapFrom(src => src.PageCount))
                                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
                                .ForMember(dest => dest.Mandatory, opt => opt.MapFrom(src => src.Mandatory))
                                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                                .ForMember(dest => dest.AllowSeeking, opt => opt.MapFrom(src => src.AllowSeeking))
                                .ForMember(dest => dest.PartCount, opt => opt.MapFrom(src => src.PartCount))
                                .ForMember(dest => dest.MinReadTimeThreshold, opt => opt.MapFrom(src => src.MinReadTimeThreshold))
                                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                .ForMember(dest => dest.ExamId, opt => opt.MapFrom(src => src.ExamId));

            CreateMap<ContentType, TrainingContentTypeDto>()
                                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dest => dest.LangCode, opt => opt.MapFrom(src => src.LangCode))
                                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));



            #endregion

            #region
            CreateMap<AddWhatYouWillLearnDto, WhatYouWillLearn>().ReverseMap();
            CreateMap<UpdateWhatYouWillLearnDto, WhatYouWillLearn>().ReverseMap();
            CreateMap<DeleteWhatYouWillLearnDto, WhatYouWillLearn>().ReverseMap();
            #endregion

            #region TrainingSection
            CreateMap<AddTrainingSectionDto, TrainingSection>().ReverseMap();
            CreateMap<UpdateTrainingSectionDto, TrainingSection>()
                                                .ForMember(dest => dest.TrainingId, opt =>
                                                    opt.Condition(src => src.TrainingId != null && src.TrainingId != 0))

                                                .ForMember(dest => dest.IsActive, opt =>
                                                    opt.Condition(src => src.IsActive != null))

                                                .ForMember(dest => dest.Title, opt =>
                                                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Title)))

                                                .ForMember(dest => dest.Abbreviation, opt =>
                                                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Abbreviation)))

                                                .ForMember(dest => dest.Code, opt =>
                                                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Code)))

                                                .ForMember(dest => dest.Description, opt =>
                                                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Description)))

                                                .ForMember(dest => dest.RowNumber, opt =>
                                                    opt.Condition(src => src.RowNumber != null && src.RowNumber != 0))

                                                .ForMember(dest => dest.LangCode, opt =>
                                                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.LangCode)));


            CreateMap<DeleteTrainingSectionDto, TrainingSection>().ReverseMap();
            CreateMap<GetTrainingSectionDto, TrainingSection>().ReverseMap();
            #endregion

            #region TrainingContent
            CreateMap<AddTrainingContentDto, TrainingContent>()
                 .ForAllMembers(opt =>
                     opt.Condition((src, dest, srcMember, context) =>
                     {
                         if (srcMember == null)
                             return false;

                         var propertyName = opt.DestinationMember.Name;
                         var fkProperties = new[] { "ContentTypeId", "TrainingSectionId" };

                         if (fkProperties.Contains(propertyName) && srcMember is long longValue && longValue == 0)
                             return false;

                         return true;
                     }));

            CreateMap<UpdateTrainingContentDto, TrainingContent>()
                        .ForAllMembers(opt =>
                          opt.Condition((src, dest, srcMember, context) =>
                          {
                              if (srcMember == null)
                                  return false;

                              var propertyName = opt.DestinationMember.Name;
                              var fkProperties = new[] { "ContentTypeId", "TrainingSectionId" };

                              if (fkProperties.Contains(propertyName) && srcMember is long longValue && longValue == 0)
                                  return false;

                              return true;
                          }));

            CreateMap<DeleteTrainingContentDto, TrainingContent>().ReverseMap();
            CreateMap<GetTrainingContentDto, TrainingContent>().ReverseMap();
            #endregion
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

            #region User

            CreateMap<GetUserResponse, User>().ReverseMap();
            CreateMap<UserSignUpDto, CurrAcc>();
            CreateMap<UserSignUpDto, SignUpDto>();
            CreateMap<AddUserDto, SignUpDto>();

            #endregion

            CreateMap<TrainingCategory, GetTrainingCategoryDto>()
            .ForMember(dest => dest.MasterCategoryTitle, opt => opt.MapFrom(src => src.MasterTrainingCategory.Title))
            .ForMember(dest => dest.MasterCategoryCode, opt => opt.MapFrom(src => src.MasterTrainingCategory.Code))
            .ForMember(dest => dest.MasterCategoryDescription, opt => opt.MapFrom(src => src.MasterTrainingCategory.Description));



            CreateMap<AddContentTypeDto, ContentType>().ReverseMap();
            CreateMap<UpdateContentTypeDto, ContentType>().ReverseMap();
            CreateMap<DeleteContentTypeDto, ContentType>().ReverseMap();

            CreateMap<AddTrainingCategoryDto, TrainingCategory>().ReverseMap();
            CreateMap<UpdateTrainingCategoryDto, TrainingCategory>().ReverseMap();
            CreateMap<DeleteTrainingCategoryDto, TrainingCategory>().ReverseMap();

            CreateMap<Instructor, UserInstructorDto>().ReverseMap();

            CreateMap<GetWhatYouWillLearnDto, WhatYouWillLearn>().ReverseMap();

            CreateMap<AddTrainingContentDto, ContentLibrary>().ReverseMap();

            CreateMap<AddContentLibraryDto, ContentLibrary>().ForAllMembers(opt =>
                         opt.Condition((src, dest, srcMember, context) =>
                         {
                             if (srcMember == null)
                                 return false;

                             var propertyName = opt.DestinationMember.Name;
                             var fkProperties = new[] { "FileTypeId" };

                             if (fkProperties.Contains(propertyName) && srcMember is long longValue && longValue == 0)
                                 return false;

                             return true;
                         }));
            CreateMap<GetFileTypeDto, FileType>().ReverseMap();



            #region ContentComment

            CreateMap<ContentCommentDto, ContentComment>();
            CreateMap<UpdateContentCommentDto, ContentComment>();
            CreateMap<AddContentCommentDto, ContentComment>();

            #endregion


            #region CommentVote

            CreateMap<CommentVoteDto, CommentVote>().ReverseMap();
            CreateMap<UpdateCommentVoteDto, CommentVote>().ReverseMap();
            CreateMap<AddCommentVoteDto, CommentVote>().ReverseMap();

            #endregion

            CreateMap<GetCurrAccTrainingDto, CurrAccTraining>().ReverseMap();
            CreateMap<AddCurrAccTrainingDto, CurrAccTraining>().ReverseMap();

            CreateMap<UpdateCurrAccTrainingDto, CurrAccTraining>()
                     .ForAllMembers(opt =>
                         opt.Condition((src, dest, srcMember, context) =>
                         {
                             if (srcMember == null)
                                 return false;

                             var propertyName = opt.DestinationMember.Name;
                             var fkProperties = new[] { "CurrAccId", "TrainingId", "CurrAccTrainingStatusId", "ConfirmUserId" };

                             if (fkProperties.Contains(propertyName) && srcMember is long longValue && longValue == 0)
                                 return false;

                             return true;
                         }));

            CreateMap<TrainingContent, TrainingContentDto>()
                .ForMember(dest => dest.TrainingContentLibraryDto, opt => opt.MapFrom(src =>
                    src.ContentLibraries.FirstOrDefault() != null
                        ? new TrainingContentLibraryDto
                        {
                            TrainingContentLibraryFileName = src.ContentLibraries.FirstOrDefault().FileName,
                            TrainingContentLibraryFilePath = src.ContentLibraries.FirstOrDefault().FilePath,
                            TrainingContentLibraryThumbnail = src.ContentLibraries.FirstOrDefault().Thumbnail,
                            TrainingContentLibraryVideoDuration = src.ContentLibraries.FirstOrDefault().VideoDuration,
                            TrainingContentLibraryDocumentFileSize = src.ContentLibraries.FirstOrDefault().DocumentFileSize,
                            TrainingContentLibraryDocumentPageCount = src.ContentLibraries.FirstOrDefault().DocumentPageCount
                        }
                        : null));
        }
    }
}
