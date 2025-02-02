

using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Settings;
using HrHub.Application.Factories;
using HrHub.Application.Helpers;
using HrHub.Application.Integrations.NotificationServices;
using HrHub.Core.Helpers;
using HrHub.Core.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
            var httpClientSettings = AppSettingsHelper.GetData<HttpClientConfiguration>();
            services.RegisterIntegration(config =>
            {
                config.HttpClients = httpClientSettings.HttpClients
                .Select(s => new HttpClientSettings
                {
                    BaseUrl = s.BaseUrl,
                    Name = s.Name,
                    EndPoints = s.EndPoints
                        .Select(ep => new EndPoint
                        {
                            Url = ep.Url,
                            Name = ep.Name

                        }).ToList()
                }).ToList();
            });


            var smsSettings = AppSettingsHelper.GetData<SmsServiceSettings>();
            services.Configure<SmsServiceSettings>(config =>
            {
                config.IsActive = smsSettings.IsActive;
                config.BaseAddress = smsSettings.BaseAddress;
                config.Username = smsSettings.Username;
                config.Password = smsSettings.Password;
                config.Originator = smsSettings.Originator;
            });

            var mailSettings = AppSettingsHelper.GetData<MailServiceSettings>();
            services.Configure<MailServiceSettings>(config =>
            {
                config.Host = mailSettings.Host;
                config.Port = mailSettings.Port;
                config.SSLPort = mailSettings.SSLPort;
                config.Username = mailSettings.Username;
                config.Password = mailSettings.Password;
            });
        }

        public static IApplicationBuilder IntegrationHelperConfig(this IApplicationBuilder app)
        {
            var httpClientFactory = app.ApplicationServices.GetService<IHttpClientFactory>();
            IntegrationHelper.IntegrationHelperConfigure(httpClientFactory);
            return app;
        }

    }
}
