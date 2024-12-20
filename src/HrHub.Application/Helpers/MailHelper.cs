using HrHub.Abstraction.Enums;
using HrHub.Core.Helpers;

namespace HrHub.Application.Helpers
{
    public static class MailHelper
    {
        public static string GenerateEmailBody(string emailTemplatePath, Dictionary<string, string> parameters)
        {
            var htmlText = "";
            using (StreamReader reader = System.IO.File.OpenText(emailTemplatePath))
            {
                htmlText = reader.ReadToEnd();
            }
            foreach (var param in parameters)
            {
                htmlText = htmlText.Replace(param.Key, param.Value);
            }
            var htmlTextBytes = System.Text.Encoding.UTF8.GetBytes(htmlText);
            return System.Convert.ToBase64String(htmlTextBytes);
        }

        public static string GetMailBody(MailType mailType, StateEnum? state = null)
        {
            string mailBody = "";
            if (mailType == MailType.VerifyEmail)
            {
                mailBody = @"<!DOCTYPE html>
<html lang=""tr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Doğrulama Kodu</title>
</head>
<body>
    <div style=""font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; border: 1px solid #ddd; border-radius: 5px;"">
        <p>HrHub Kayıt</p>
        <p>Doğrulama Kodu  : <strong>@VERIFYCODE</strong></p>
    </div>
</body>
</html>";
            }
            else if (mailType == MailType.AddUser)
            {
                mailBody = @"<!DOCTYPE html>
<html lang=""tr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Doğrulama Kodu</title>
</head>
<body>
    <div style=""font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; border: 1px solid #ddd; border-radius: 5px;"">
        <p>HrHub Giriş Bilgileri</p>
        <p>Kullanıcı  : <strong>@USERMANE</strong></p>
        <p>Parola  : <strong>@PASSWORD</strong></p>
    </div>
</body>
</html>";
            }
            return mailBody;

        }

    }
}
