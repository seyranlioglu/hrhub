namespace HrHub.Domain.Contracts.Dtos.NotificationDtos
{
    public class SendSmsDto
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Originator { get; set; }
        public List<SmsMessage> SmsMessages { get; set; }

    }
    public class SmsMessage
    {
        public string Messagetext { get; set; }
        public string Recipient { get; set; }
    }
}
