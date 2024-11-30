using HrHub.Abstraction.Enums;
using System.Collections.Concurrent;
using static ServiceStack.Diagnostics.Events;

namespace HrHub.Application.Helpers
{
    public static class VerificationHelper
    {
        private static readonly ConcurrentDictionary<string, string> _codes = new();
        private static readonly TimeSpan CodeExpiry = TimeSpan.FromMinutes(2);

        public static async void SaveCode(string codeParameter, string code)
        {
            _codes[codeParameter] = code;
            await Task.Delay(CodeExpiry);
            _codes.TryRemove(codeParameter, out _);
        }

        public static string? GetCode(string codeParameter)
        {
            return _codes.TryGetValue(codeParameter, out var code) ? code : null;
        }
        public static string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();
        }
        public static string MaskPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length < 4)
                throw new ArgumentException("Telefon numarası en az 4 hane olmalı.");

            phoneNumber = phoneNumber.Replace(" ", "");
            var maskedPart = new string('*', phoneNumber.Length - 4);
            var visiblePart = phoneNumber.Substring(phoneNumber.Length - 4);

            return $"{maskedPart}{visiblePart}";
        }
        public static string MaskEmail(string email)
        {
            var parts = email.Split('@');
            if (parts.Length != 2)
                throw new ArgumentException("Geçerli bir e-posta adresi değil.");

            var username = parts[0];
            var domain = parts[1];

            var maskedUsername = username.Length > 1
                ? username[0] + new string('*', username.Length - 1)
                : "*";

            var maskedDomain = domain.Length > 3
                ? domain.Substring(0, 3) + "..."
                : domain;

            return $"{maskedUsername}@{maskedDomain}";
        }
        public static void SendVerifyCode(string receiver, string message, SubmissionTypeEnum type)
        {
            switch (type)
            {
                case SubmissionTypeEnum.Email:
                    //TODO: Mail gönderme işlemleri yapılacak
                    break;
                case SubmissionTypeEnum.Sms:
                    //TODO: Sms gönderme işlemleri yapılacak
                    break;
            }
        }
    }
}
