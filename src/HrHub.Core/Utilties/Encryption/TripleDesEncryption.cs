using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HrHub.Core.Utilties.Encryption
{
    public class TripleDesEncryption
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString().ToUpper();
        }

        public static byte[] StringToByteArray(string hex)
        {
            int charNum = hex.Length;
            byte[] bytes = new byte[charNum / 2];
            for (int i = 0; i < charNum; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string Encrypt(string TextToEncrypt, string securityKey)
        {
            byte[] MyEncryptedArray = Encoding.UTF8
               .GetBytes(TextToEncrypt);

            MD5CryptoServiceProvider MyMD5CryptoService = new
               MD5CryptoServiceProvider();

            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash
               (Encoding.UTF8.GetBytes(securityKey));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new
               TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService
               .CreateEncryptor();

            byte[] MyresultArray = MyCrytpoTransform
               .TransformFinalBlock(MyEncryptedArray, 0,
               MyEncryptedArray.Length);

            MyTripleDESCryptoService.Clear();

            return Convert.ToBase64String(MyresultArray, 0,
               MyresultArray.Length);
        }

        public static string Decrypt(string TextToDecrypt, string securityKey)
        {
            byte[] MyDecryptArray = Convert.FromBase64String
               (TextToDecrypt);

            MD5CryptoServiceProvider MyMD5CryptoService = new
               MD5CryptoServiceProvider();

            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash
               (Encoding.UTF8.GetBytes(securityKey));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new
               TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService
               .CreateDecryptor();

            byte[] MyresultArray = MyCrytpoTransform
               .TransformFinalBlock(MyDecryptArray, 0,
               MyDecryptArray.Length);

            MyTripleDESCryptoService.Clear();

            return Encoding.UTF8.GetString(MyresultArray);
        }

        public static string EncryptCustom(string textToEncrypt, string securityKey, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.Zeros, byte[]? ivBytes = null)
        {
            byte[] plainTextBytes = StringToByteArray(textToEncrypt);
            byte[] keyBytes = StringToByteArray(securityKey);

            using (TripleDES desCryptoProvider = TripleDES.Create())
            {
                desCryptoProvider.Key = keyBytes;
                desCryptoProvider.Mode = cipherMode;
                desCryptoProvider.Padding = paddingMode;
                if (cipherMode == CipherMode.CBC)
                    desCryptoProvider.IV = ivBytes == default ? new byte[8] : ivBytes;

                using (ICryptoTransform cryptoTransform = desCryptoProvider.CreateEncryptor())
                {
                    byte[] encryptedBytes = cryptoTransform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
                    return ByteArrayToString(encryptedBytes);
                }
            }
        }

        public static string DecryptCustom(string textToDecrypt, string securityKey, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.Zeros, byte[]? ivBytes = default)
        {
            byte[] encryptedBytes = StringToByteArray(textToDecrypt);
            byte[] keyBytes = StringToByteArray(securityKey);
            using (TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider())
            {
                desCryptoProvider.Key = keyBytes;
                desCryptoProvider.Mode = cipherMode;
                desCryptoProvider.Padding = paddingMode;
                if (cipherMode is CipherMode.CBC)
                    desCryptoProvider.IV = ivBytes == default ? new byte[8] : ivBytes;

                using (ICryptoTransform cryptoTransform = desCryptoProvider.CreateDecryptor())
                {
                    byte[] decryptedBytes = cryptoTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return ByteArrayToString(decryptedBytes);
                }
            }
        }
    }
}

