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
    public class AddTrainingBusinessRule : IAddTrainingBusinesRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Training> trainingRepository;
        private readonly Repository<Instructor> instructorRepository;
        private readonly Repository<TrainingCategory> categoryRepository;
        private readonly Repository<TrainingLevel> levelRepository;
        private readonly Repository<TimeUnit> timeUnitRepository;
        public AddTrainingBusinessRule(IHrUnitOfWork unitOfWork
                                      )
        {
            this.unitOfWork = unitOfWork;
            this.trainingRepository = unitOfWork.CreateRepository<Training>();
            this.instructorRepository = unitOfWork.CreateRepository<Instructor>();
            this.categoryRepository = unitOfWork.CreateRepository<TrainingCategory>();
            this.levelRepository = unitOfWork.CreateRepository<TrainingLevel>();
            this.timeUnitRepository = unitOfWork.CreateRepository<TimeUnit>();
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

                var isInstructorExist = instructorRepository
                              .Exists(p =>
                                  p.Id == trainingDto.InstructorId);
                if (!isInstructorExist)
                    return (false, ValidationMessages.InstructorNotFound);
                var isCategoryExist = categoryRepository
                                            .Exists(p =>
                                                p.Id == trainingDto.CategoryId);
                if (!isCategoryExist)
                    return (false, ValidationMessages.CategoryNotFound);
                var islevelExist = levelRepository
                                      .Exists(p =>
                                          p.Id == trainingDto.TrainingLevelId);
                if (!islevelExist)
                    return (false, ValidationMessages.TrainingLevelNotFound);
                var isTimeUnitExist = timeUnitRepository
                                      .Exists(p =>
                                          p.Id == trainingDto.CompletionTimeUnitId);
                if (!isTimeUnitExist)
                    return (false, ValidationMessages.TimeUnitNotFound);

                if (trainingDto.Amount < 0)
                    return (false, ValidationMessages.AmountMustBeGreaterThanZero);

                if (trainingDto.CurrentAmount < 0)
                    return (false, ValidationMessages.CurrentAmountMustBeGreaterThanZero);

                if (trainingDto.CompletionTime is null || trainingDto.CompletionTimeUnitId < 0)
                    return (false, ValidationMessages.CompletionTimeAndUnitMustBeProvided);
            }

            return (true, string.Empty);
        }
    }
}
