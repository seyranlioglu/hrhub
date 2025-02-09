using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

public class DeleteCurrAccTrainingBusinessRule : IDeleteCurrAccTrainingBusinessRule
{
    private readonly IHrUnitOfWork hrUnitOfWork;
    private readonly Repository<CurrAccTraining> currAccTrainingRepository;

    public DeleteCurrAccTrainingBusinessRule(IHrUnitOfWork hrUnitOfWork)
    {
        this.hrUnitOfWork = hrUnitOfWork;
        this.currAccTrainingRepository = hrUnitOfWork.CreateRepository<CurrAccTraining>();
    }

    public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
    {
        if (value is long trainingId && trainingId > 0)
        {
            var trainingExists = currAccTrainingRepository.Exists(predicate: p => p.Id == trainingId);
            if (!trainingExists)
                return (false, "Silinmek istenen eğitim bulunamadı!");

        }

        return (true, string.Empty);
    }
}
