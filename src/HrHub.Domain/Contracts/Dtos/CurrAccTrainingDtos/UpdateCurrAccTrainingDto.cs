namespace HrHub.Domain.Contracts.Dtos.CurrAccTrainingDtos
{
    public class UpdateCurrAccTrainingDto
    {
        public long Id { get; set; }
        public long? CurrAccId { get; set; }
        public long? TrainingId { get; set; }
        public string? CurrAccTrainingStatusCode { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public long? ConfirmUserId { get; set; }
        public string ConfirmNotes { get; set; }
        public int? LicenceCount { get; set; }
        public long? CartItemId { get; set; }
    }
}
