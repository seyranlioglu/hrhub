using HrHub.Abstraction.Exceptions;
using HrHub.Abstraction.Notification;
using HrHub.Abstraction.Notification.BaseModel;
using HrHub.Domain.Contracts.Dtos.NotificationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Integrations.NotificationServices
{
    public class EmailService : IMessageSender
    {
        public void Send(IMessage message)
        {
            if (message != null && message is EmailMessageDto smsMessage)
            {
                //Sms gönderim işlemleri yapılacak
            }
            else
                throw new BusinessException("Invalid message type for Email sender.");
        }

        public Task SendAsync(IMessage message)
        {
            if (message != null && message is EmailMessageDto smsMessage)
            {
                //Sms gönderim işlemleri yapılacak
                return Task.CompletedTask;
            }
            else
                throw new BusinessException("Invalid message type for Email sender.");
        }
    }
}
