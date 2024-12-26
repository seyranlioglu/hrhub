using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingBusinessRules
{
    public class AddTrainingBusinessRule : IAddTrainingBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Training> trainingRepository;
        private readonly Repository<Instructor> instructorRepository;
        private readonly Repository<TrainingCategory> categoryRepository;
        private readonly Repository<TrainingLevel> levelRepository;
        private readonly Repository<TimeUnit> timeUnitRepository;
        private readonly Repository<Precondition> preconditionRepository;
        private readonly Repository<ForWhom> forWhomRepository;
        private readonly Repository<EducationLevel> educationLevelRepository;
        private readonly Repository<PriceTier> priceTierRepository;
        public AddTrainingBusinessRule(IHrUnitOfWork unitOfWork
                                      )
        {
            this.unitOfWork = unitOfWork;
            this.trainingRepository = unitOfWork.CreateRepository<Training>();
            this.instructorRepository = unitOfWork.CreateRepository<Instructor>();
            this.categoryRepository = unitOfWork.CreateRepository<TrainingCategory>();
            this.levelRepository = unitOfWork.CreateRepository<TrainingLevel>();
            this.timeUnitRepository = unitOfWork.CreateRepository<TimeUnit>();
            this.preconditionRepository = unitOfWork.CreateRepository<Precondition>();
            this.forWhomRepository = unitOfWork.CreateRepository<ForWhom>();
            this.educationLevelRepository = unitOfWork.CreateRepository<EducationLevel>();
            this.priceTierRepository = unitOfWork.CreateRepository<PriceTier>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is AddTrainingDto trainingDto && trainingDto is not null)
            {
                var isTrainingExist = trainingRepository
                    .Exists(
                    predicate: p => p.Title == trainingDto.Title
                                    && p.Description == trainingDto.Description
                                    && p.Code == trainingDto.Code
                                    && p.Abbreviation == trainingDto.Abbreviation);
                if (isTrainingExist)
                    return (false, ValidationMessages.TrainingExistsError);

                //var isInstructorExist = instructorRepository
                //              .Exists(p =>
                //                  p.Id == trainingDto.InstructorId);
                //if (!isInstructorExist)
                //    return (false, ValidationMessages.InstructorNotFound);
                var isCategoryExist = categoryRepository
                                            .Exists(p =>
                                                p.Id == trainingDto.CategoryId);
                if (!isCategoryExist)
                    return (false, ValidationMessages.CategoryNotFound);

                //var isPreconditionExist = preconditionRepository
                //                       .Exists(p =>
                //                           p.Id == trainingDto.PreconditionId);
                //if (!isPreconditionExist)
                //    return (false, ValidationMessages.PreconditionNotFound);

                //var isForWhomExist = forWhomRepository
                //                  .Exists(p =>
                //                      p.Id == trainingDto.ForWhomId);
                //if (!isForWhomExist)
                //    return (false, ValidationMessages.ForWhomNotFound);

                //var islevelExist = levelRepository
                //                      .Exists(p =>
                //                          p.Id == trainingDto.TrainingLevelId);
                //if (!islevelExist)
                //    return (false, ValidationMessages.TrainingLevelNotFound);

                //var isEducationLevelExist = educationLevelRepository
                //                 .Exists(p =>
                //                     p.Id == trainingDto.EducationLevelId);
                //if (!isEducationLevelExist)
                //    return (false, ValidationMessages.EducationLevelNotFound);

                //var isPriceTierExist = priceTierRepository
                //            .Exists(p =>
                //                p.Id == trainingDto.PriceTierId);
                //if (!isPriceTierExist)
                //    return (false, ValidationMessages.PriceTierNotFound);

                //var isTimeUnitExist = timeUnitRepository
                //                      .Exists(p =>
                //                          p.Id == trainingDto.CompletionTimeUnitId);

                //if (!isTimeUnitExist)
                //    return (false, ValidationMessages.TimeUnitNotFound);

                //if (trainingDto.Amount < 0)
                //    return (false, ValidationMessages.AmountMustBeGreaterThanZero);

                //if (trainingDto.CurrentAmount < 0)
                //    return (false, ValidationMessages.CurrentAmountMustBeGreaterThanZero);

                //if (trainingDto.CompletionTime is null || trainingDto.CompletionTimeUnitId < 0)
                //    return (false, ValidationMessages.CompletionTimeAndUnitMustBeProvided);
            }

            return (true, string.Empty);
        }
    }
}
