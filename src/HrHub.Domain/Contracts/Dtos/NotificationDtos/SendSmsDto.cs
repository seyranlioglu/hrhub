namespace HrHub.Domain.Contracts.Dtos.NotificationDtos
{
    public class SendSmsDto
    {
        public string BlackListFilter { get; set; }
        public string BrandCode { get; set; }
        public string BroadCastMessage { get; set; }
        public string SmsMessages { get; set; }
        public string Channel { get; set; }
        public int IysFilter { get; set; }
        public string Originator { get; set; }
        public string Password { get; set; }
        public string RecipientType { get; set; }
        public string RetailerCode { get; set; }
        public string Username { get; set; }
    }
}
