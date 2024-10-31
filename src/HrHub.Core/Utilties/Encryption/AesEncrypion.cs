using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HrHub.Core.Utilties.Encryption
{
    public class AesEncrypion
    {
        private const string Password = "SOWJDF*876olf89Ol";
        private const string HashAlgorithm = "SHA256";
        private const string Salt = "hkp=34Mn_ewr";
        private const string InitialVector = "AKL397m39-54JRsU";
        private const int PasswordIterations = 2;
        private const int KeySize = 256;

        private static string Encrypt(string PlainText, string Password,
            string Salt, string HashAlgorithm,
            int PasswordIterations, string InitialVector,
            int KeySize)
        {
            if (string.IsNullOrEmpty(PlainText))
                return "";

            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] CipherTextBytes = null;
            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                        CryptoStream.FlushFinalBlock();
                        CipherTextBytes = MemStream.ToArray();
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
            SymmetricKey.Clear();
            return Convert.ToBase64String(CipherTextBytes);
        }

        private static string Decrypt(string CipherText, string Password,
            string Salt, string HashAlgorithm,
            int PasswordIterations, string InitialVector,
            int KeySize)
        {
            if (string.IsNullOrEmpty(CipherText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
            int ByteCount = 0;
            using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                    {

                        ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
            SymmetricKey.Clear();
            return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
        }

        public static string EncryptString(string PlainText)
        {
            string rsltFunc = "";

            try
            {
                rsltFunc = Encrypt(PlainText, Password, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize);
            }
            catch
            {
                rsltFunc = "";
            }

            return rsltFunc;
        }

        public static string DecryptString(string CipherText)
        {
            string rsltFunc = "";

            try
            {
                rsltFunc = Decrypt(CipherText, Password, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize);
            }
            catch
            {
                rsltFunc = "";
            }

            return rsltFunc;
        }
    }
}
