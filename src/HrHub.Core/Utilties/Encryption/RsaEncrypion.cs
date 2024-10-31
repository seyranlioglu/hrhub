using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Utilties.Encryption
{
    public class RsaEncrypion
    {
        public static string Encrypt(string data, string xmlKeyPath)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                using (TextReader textReader = new StreamReader(xmlKeyPath))
                {
                    var xml = textReader.ReadToEnd();
                    rsa.FromXmlString(xml);
                    byte[] byteData = Encoding.UTF8.GetBytes(data);
                    var resArr = rsa.Encrypt(byteData, RSAEncryptionPadding.Pkcs1);
                    return Convert.ToBase64String(resArr);
                }
            }
        }

        public static string Decrypt(string text, string xmlKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                using (TextReader textReader = new StreamReader(xmlKeyPath))
                {
                    var xml = textReader.ReadToEnd();
                    rsa.FromXmlString(xml);
                    var dataArr = Convert.FromBase64String(text);
                    byte[] resArr = rsa.Decrypt(dataArr, RSAEncryptionPadding.Pkcs1);
                    return Encoding.UTF8.GetString(resArr);
                }
            }
        }

        static byte[] HexStringToByteArray(string hex)
        {
            hex = hex.Replace(":", "").Replace(" ", ""); // Gereksiz karakterleri kaldırın
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static Tuple<string, string> CreateKey()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Genel anahtarı al
                string publicKey = rsa.ToXmlString(false);
                Console.WriteLine("Public Key:\n" + publicKey);

                // Özel anahtarı al
                string privateKey = rsa.ToXmlString(true);
                Console.WriteLine("\nPrivate Key:\n" + privateKey);
                return Tuple.Create(publicKey, privateKey);
            }
        }
    }
}
