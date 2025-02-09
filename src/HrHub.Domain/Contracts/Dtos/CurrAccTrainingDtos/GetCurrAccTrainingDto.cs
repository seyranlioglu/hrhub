namespace HrHub.Domain.Contracts.Dtos.CurrAccTrainingDtos
{
    public class GetCurrAccTrainingDto
    {
        public long Id { get; set; }
        //CurrAcc
        public string? CurrAccName { get; set; }
        public string? CurrAccSurname { get; set; }
        public string CurrAccTitle { get; set; }
        public string CurrAccAddress { get; set; }
        public string CurrAccTaxNumber { get; set; }
        public string CurrAccIdentityNumber { get; set; }
        public string CurrAccPostalCode { get; set; }


        //Training
        public string TrainingCode { get; set; }
        public string TrainingTitle { get; set; }
        public string TrainingDescription { get; set; }


        public string CurrAccTrainingStatusCode { get; set; }
        public string CurrAccTrainingStatusTitle { get; set; }
        public string CurrAccTrainingStatusDescription { get; set; }


        public DateTime ConfirmDate { get; set; }

        //User
        public string UserUserName { get; set; }
        public string UserName { get; set; }
        public string UserSurName { get; set; }

        public string ConfirmNotes { get; set; }
        public int LicenceCount { get; set; }
        public long CartItemId { get; set; }
    }
}
