using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingContentBusinessRules
{
    public class UpdateTrainingContentBusinessRule : IUpdateTrainingContentBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingContent> trainingContentRepository;
        private readonly Repository<ContentType> contentTypeRepository;
        private readonly Repository<TrainingSection> trainingSectionRepository;
        private readonly Repository<ContentLibrary> contentLibraryRepository;
        public UpdateTrainingContentBusinessRule(IHrUnitOfWork hrUnitOfWork
                                              )
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingContentRepository = hrUnitOfWork.CreateRepository<TrainingContent>();
            this.contentTypeRepository =hrUnitOfWork.CreateRepository<ContentType>();
            this.trainingSectionRepository = hrUnitOfWork.CreateRepository<TrainingSection>();
            this.contentLibraryRepository = hrUnitOfWork.CreateRepository<ContentLibrary>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is UpdateTrainingContentDto trainingContentDto && trainingContentDto is not null)
            {
                var isContentTypeExist = contentTypeRepository.Exists(predicate: p => p.Id == trainingContentDto.ContentTypeId);
                if (!isContentTypeExist)
                    return (false, ValidationMessages.TrainingContentTypeNotExistsError);

                var isTrainingSectionExist = trainingSectionRepository.Exists(predicate: p => p.Id == trainingContentDto.TrainingSectionId);
                if (!isTrainingSectionExist)
                    return (false, ValidationMessages.TrainingSectionNotExistsError);

                var isTrainingContentExist = trainingContentRepository.Exists(predicate: p => p.Id == trainingContentDto.Id);
                if (!isTrainingContentExist)
                    return (false, ValidationMessages.TrainingContentNotExistsError);


            }
            return (true, string.Empty);
        }
    }
}
