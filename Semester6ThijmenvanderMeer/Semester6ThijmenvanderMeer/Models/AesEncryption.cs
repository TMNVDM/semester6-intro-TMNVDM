using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Semester6ThijmenvanderMeer.Models
{
    public class AesEncryption
    {
        // 32 bytes voor AES-256
        private static readonly string Key = "12345678901234567890123456789012"; // 32 tekens
        private static readonly string IV = "1234567890123456"; // 16 tekens

        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    // Zet de geëncrypteerde byte-array om naar een Base64-string
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
}
