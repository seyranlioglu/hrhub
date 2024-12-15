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
    public class SmsService : IMessageSender
    {
        public void Send(IMessage message)
        {
            if (message != null && message is SmsMessageDto smsMessage)
            {
                //Sms gönderim işlemleri yapılacak
            }
            else
                throw new BusinessException("Invalid message type for SMS sender.");
        }

        public Task SendAsync(IMessage message)
        {
            if (message != null && message is SmsMessageDto smsMessage)
            {
                //Sms gönderim işlemleri yapılacak
                return Task.CompletedTask;
            }
            else
                throw new BusinessException("Invalid message type for SMS sender.");
        }
    }
}
