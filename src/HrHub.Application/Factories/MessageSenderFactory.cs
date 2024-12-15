using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Exceptions;
using HrHub.Abstraction.Notification;
using HrHub.Application.Integrations.NotificationServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Factories
{
    public class MessageSenderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<MessageType, Type> _senderMappings = new();
        public MessageSenderFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void RegisterSender<TSender>(MessageType messageType) where TSender : IMessageSender
        {
            _senderMappings[messageType] = typeof(TSender);
        }

        public IMessageSender GetSender(MessageType messageType)
        {
            if (_senderMappings.TryGetValue(messageType, out var sender))
            {
                return (IMessageSender)serviceProvider.GetRequiredService(sender);
            }

            throw new BusinessException($"No sender registered for message type {messageType}");
        }
    }
}
