using HrHub.Abstraction.Exceptions;
using HrHub.Abstraction.Notification;
using HrHub.Abstraction.Notification.BaseModel;
using HrHub.Abstraction.Settings;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.NotificationDtos;
using MimeKit;
using ServiceStack;
using System.Net;
using System.Net.Mail;

namespace HrHub.Application.Integrations.NotificationServices
{
    public class EmailService : IMessageSender
    {
        public void Send(IMessage message)
        {
            if (message != null && message is EmailMessageDto emailMessage)
            {
                var smtpSettings = AppSettingsHelper.GetData<MailServiceSettings>();
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("Osman Burnak", smtpSettings.Username));
                mimeMessage.To.Add(new MailboxAddress("Osman Burnak", emailMessage.Recipient));
                mimeMessage.Subject = emailMessage.MessageTemplate.ToString();
                if (!emailMessage.Parameters.IsNullOrEmpty())
                {
                    foreach (var item in emailMessage.Parameters)
                    {
                        emailMessage.Content = emailMessage.Content.Replace("" + item.Key + "", "" + item.Value + "");
                    }
                }
                mimeMessage.Body = new TextPart("html")
                {
                    Text = emailMessage.Content
                };
                try
                {
                    using var smtp = new MailKit.Net.Smtp.SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate(smtpSettings.Username, smtpSettings.Password);
                    smtp.Send(mimeMessage);
                    smtp.Disconnect(true);
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.Message, exp.InnerException);
                }

            }
            else
                throw new BusinessException("Invalid message type for Email sender.");
        }

        public async Task SendAsync(IMessage message)
        {
            if (message != null && message is EmailMessageDto emailMessage)
            {
                var smtpSettings = AppSettingsHelper.GetData<MailServiceSettings>();
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("HrHub", smtpSettings.Username));
                mimeMessage.To.Add(new MailboxAddress(emailMessage.Recipient, emailMessage.Recipient));
                mimeMessage.Subject = emailMessage.MessageTemplate.ToString();
                if(!emailMessage.Parameters.IsNullOrEmpty())
                {
                    foreach (var item in emailMessage.Parameters)
                    {
                        emailMessage.Content = emailMessage.Content.Replace("" + item.Key + "", "" + item.Value + "");
                    }   
                }
                mimeMessage.Body = new TextPart("html")
                {
                    Text = emailMessage.Content
                };
                try
                {
                    using var smtp = new MailKit.Net.Smtp.SmtpClient();
                    await smtp.ConnectAsync(smtpSettings.Host, smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
                    await smtp.SendAsync(mimeMessage);
                    await smtp.DisconnectAsync(true);
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.Message, exp.InnerException);
                }
            }
            else
                throw new BusinessException("Invalid message type for Email sender.");
        }
    }
}
