using HrHub.Abstraction.Enums;
using HrHub.Application.Factories;
using HrHub.Application.Integrations.NotificationServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Container.Bootstrappers
{
    public static class NotificationBootstrapper
    {
        public static void RegisterNotificationService(this IServiceCollection services)
        {
            services.AddTransient<SmsService>();
            services.AddTransient<EmailService>();

            services.AddSingleton<MessageSenderFactory>(sp =>
            {
                var factory = new MessageSenderFactory(sp);
                factory.RegisterSender<SmsService>(MessageType.Sms);
                factory.RegisterSender<EmailService>(MessageType.Email);
                return factory;
            });
        }
    }
}
