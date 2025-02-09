using HrHub.Abstraction.Consts;
using HrHub.Application.BusinessRules.CurrAccTrainingBusinesRules;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CurrAccTrainingDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

public class UpdateCurrAccTrainingBusinessRule : IUpdateCurrAccTrainingBusinessRule
{
    private readonly IHrUnitOfWork hrUnitOfWork;
    private readonly Repository<CurrAcc> currAccRepository;
    private readonly Repository<Training> trainingRepository;
    private readonly Repository<CurrAccTrainingStatus> trainingStatusRepository;
    private readonly Repository<User> userRepository;
    private readonly Repository<CurrAccTraining> currAccTrainingRepository;

    public UpdateCurrAccTrainingBusinessRule(IHrUnitOfWork hrUnitOfWork)
    {
        this.hrUnitOfWork = hrUnitOfWork;
        this.currAccRepository = hrUnitOfWork.CreateRepository<CurrAcc>();
        this.trainingRepository = hrUnitOfWork.CreateRepository<Training>();
        this.trainingStatusRepository = hrUnitOfWork.CreateRepository<CurrAccTrainingStatus>();
        this.userRepository = hrUnitOfWork.CreateRepository<User>();
        this.currAccTrainingRepository = hrUnitOfWork.CreateRepository<CurrAccTraining>();
    }

    public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
    {
        if (value is UpdateCurrAccTrainingDto trainingDto && trainingDto is not null)
        {
            // Kayıt Var mı?
            var existingTraining = currAccTrainingRepository.Exists(predicate: p => p.Id == trainingDto.Id);
            if (!existingTraining)
                return (false, ValidationMessages.TrainingNotExistsError);

            //var isCurrAccExist = currAccRepository.Exists(predicate: p => p.Id == trainingDto.CurrAccId);
            //if (!isCurrAccExist)
            //    return (false, ValidationMessages.CurrAccNotExistsError);

            //var isTrainingExist = trainingRepository.Exists(predicate: p => p.Id == trainingDto.TrainingId);
            //if (!isTrainingExist)
            //    return (false, ValidationMessages.TrainingNotExistsError);

            //var isStatusExist = trainingStatusRepository.Exists(predicate: p => p.Code == trainingDto.CurrAccTrainingStatusCode);
            //if (!isStatusExist)
            //    return (false, ValidationMessages.TrainingStatusNotExistsError);

            //var isConfirmUserExist = userRepository.Exists(predicate: p => p.Id == trainingDto.ConfirmUserId);
            //if (!isConfirmUserExist)
            //    return (false, ValidationMessages.ConfirmUserNotExistsError);        
        }

        return (true, string.Empty);
    }
}

