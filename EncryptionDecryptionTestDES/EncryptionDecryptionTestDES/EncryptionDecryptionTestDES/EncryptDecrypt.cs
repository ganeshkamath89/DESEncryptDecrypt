using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionDecryptionTestDES
{
    class EncryptDecrypt
    {
        private const string key = @"DASH1234";

        /// <summary>
        /// Encrypts given text using DES cryptography
        /// </summary>
        /// <param name="text">Text to be enrypted</param>
        /// <returns>Encrypted text</returns>
        public static string Encrypt(string text)
        {
            string encryptedText = string.Empty;
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
                using (StreamWriter writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(text);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();
                    encryptedText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
            return encryptedText;
        }

        /// <summary>
        /// Decrypts given text using DES cryptography
        /// </summary>
        /// <param name="text">Text to be decrypted</param>
        /// <returns>Decrypted text</returns>
        public static string Decrypt(string text)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(text)))
            {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Encrypts the given file using DES cryptography
        /// </summary>
        /// <param name="filename">File to be encrypted</param>
        public static void EncryptFile(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                ICryptoTransform desEncrypt = des.CreateEncryptor();
                using (CryptoStream cryptostream = new CryptoStream(stream, desEncrypt, CryptoStreamMode.Write))
                {
                    byte[] bytearrayinput = new byte[stream.Length];
                    stream.Read(bytearrayinput, 0, bytearrayinput.Length);
                    // Clear content
                    stream.SetLength(0);
                    // Write encrypted content
                    cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                }
            }
        }

        /// <summary>
        /// Decrypts given file using DES cryptography
        /// </summary>
        /// <param name="filename">File to ve decrypted</param>
        public static void DecryptFile(string filename)
        {
            var DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.ASCII.GetBytes(key);
            DES.IV = Encoding.ASCII.GetBytes(key);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            var ms = new MemoryStream();
            using (var fsread = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read))
                {
                    int data;
                    fsread.Flush();
                    while ((data = cryptostreamDecr.ReadByte()) != -1)
                    {
                        ms.WriteByte((byte)data);
                    }
                    cryptostreamDecr.Close();
                }
                using (var fsWrite = new FileStream(filename, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    ms.WriteTo(fsWrite);
                    ms.Flush();
                }
            }
        }
    }
}
