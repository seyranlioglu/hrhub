using System.Security.Cryptography;
using System.Text;

namespace HrHub.Core.Utilties.Encryption
{
    public static class ShaEncryption
    {
        public static string ComputeHMACSHA256Hash(string data, string salt)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException("Data and salt cannot be null or empty.");
            }

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Byte dizisini string olarak dönüştürme
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2")); // Her byte'ı hexadecimal olarak dönüştür
                }

                return builder.ToString();
            }
        }
    }
}
