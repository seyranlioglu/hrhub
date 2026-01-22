using System.Security.Cryptography;
using System.Text;

namespace HrHub.Application.Helpers
{
    public static class PasswordHelper
    {
        public static string GeneratePassword(int length, bool includeUppercase, bool includeNumbers, bool includeSpecialChars)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Şifre uzunluğu 0 veya daha az olamaz.");
            }

            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numberChars = "0123456789";
            const string specialChars = "!@#$%^&*_-?";
            StringBuilder allowedChars = new StringBuilder(lowercaseChars);

            if (includeUppercase)
            {
                allowedChars.Append(uppercaseChars);
            }

            if (includeNumbers)
            {
                allowedChars.Append(numberChars);
            }

            if (includeSpecialChars)
            {
                allowedChars.Append(specialChars);
            }

            if (allowedChars.Length == 0)
            {
                throw new ArgumentException("En az bir karakter türü seçmelisiniz.");
            }

            StringBuilder password = new StringBuilder(length);
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumber = new byte[4];

                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(randomNumber);
                    int index = (randomNumber[0] % allowedChars.Length);
                    password.Append(allowedChars[index]);
                }
            }

            return password.ToString();
        }
    }
}
