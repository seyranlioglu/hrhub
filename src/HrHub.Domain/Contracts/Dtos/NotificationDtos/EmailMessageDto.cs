using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Notification.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.NotificationDtos
{
    public class EmailMessageDto : IMessage
    {
        public MessageTemplates MessageTemplate { get; set; }
        public string Recipient { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
