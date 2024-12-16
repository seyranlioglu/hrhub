using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Exceptions;
using HrHub.Abstraction.Factories;
using HrHub.Abstraction.Notification;
using HrHub.Abstraction.Notification.BaseModel;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.Settings;
using HrHub.Application.Helpers;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.NotificationDtos;
using ServiceStack.Web;

namespace HrHub.Application.Integrations.NotificationServices
{
    public class SmsService : IMessageSender
    {
        private readonly IHttpClientHelperFactory httpClientHelperFactory;

        public SmsService()
        {
            this.httpClientHelperFactory = new HttpClientHelperFactory(IntegrationHelper.CreateHttpClient(Abstraction.Enums.HttpClients.SmsServer.ToString()));
        }

        public void Send(IMessage message)
        {
            if (message != null && message is SmsMessageDto smsMessage)
            {
                var smsSettings = AppSettingsHelper.GetData<SmsServiceSettings>();
                if (smsSettings.IsActive && smsSettings != null)
                {
                    var sendSmsModel = new SendSmsDto
                    {
                        BlackListFilter = smsSettings.BlackListFilter,
                        BrandCode = smsSettings.BrandCode,
                        BroadCastMessage = smsMessage.Recipient,
                        SmsMessages = smsMessage.Content,
                        Channel = smsSettings.Channel,
                        IysFilter = smsSettings.IysFilter,
                        Originator = smsSettings.Originator,
                        Password = smsSettings.Password,
                        RecipientType = smsSettings.RecipientType,
                        RetailerCode = smsSettings.RetailerCode,
                        Username = smsSettings.Username
                    };



                    var client = AppSettingsHelper.GetData<Abstraction.Settings.HttpClientConfiguration>().HttpClients
                           .Find(w => w.Name == Abstraction.Enums.HttpClients.SmsServer.ToString());
                    if (client == null)
                    {
                        throw new InvalidOperationException("Client could not be found.");
                    }
                    var endPoint = client.EndPoints.Find(w => w.Name == EndPoints.SendSms.ToString());
                    if (endPoint is null)
                    {
                        throw new InvalidOperationException("Endpoint could not be found.");
                    }
                    httpClientHelperFactory.PostAsync<Response<object>, SendSmsDto>(endPoint.Url, sendSmsModel);
                }
                else
                    throw new BusinessException("Invalid message type for SMS sender.");
            }
        }

        public async Task SendAsync(IMessage message)
        {
            if (message != null && message is SmsMessageDto smsMessage)
            {
                var smsSettings = AppSettingsHelper.GetData<SmsServiceSettings>();
                if (smsSettings.IsActive && smsSettings != null)
                {

                    var client = AppSettingsHelper.GetData<Abstraction.Settings.HttpClientConfiguration>().HttpClients
                           .Find(w => w.Name == Abstraction.Enums.HttpClients.SmsServer.ToString());
                    if (client == null)
                    {
                        throw new InvalidOperationException("Client could not be found.");
                    }
                    var endPoint = client.EndPoints.Find(w => w.Name == EndPoints.SendSms.ToString());
                    if (endPoint is null)
                    {
                        throw new InvalidOperationException("Endpoint could not be found.");
                    }
                    await httpClientHelperFactory.PostAsync<Response<CommonResponse>, object>(endPoint.Url, "");
                }
            }
            else
                throw new BusinessException("Invalid message type for SMS sender.");
        }
    }
}
