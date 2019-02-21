using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionDecryptionTestDES {
    class EncryptDecrypt {
        private const string key = @"DASH1234";

        /// Encrypts given text using DES cryptography
        public static string Encrypt(string text) {
            string encryptedText = string.Empty;
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider()) {
                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write)) {
                        using (StreamWriter writer = new StreamWriter(cryptoStream)) {
                            writer.Write(text);
                            writer.Flush();
                            cryptoStream.FlushFinalBlock();
                            encryptedText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        }
                    }
                }
            }
            return encryptedText;
        }

        /// Decrypts given text using DES cryptography
        public static string Decrypt(string text) {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider()) {
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(text))) {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read)) {
                        using (StreamReader reader = new StreamReader(cryptoStream)) {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// Encrypts the given file using DES cryptography
        public static void EncryptFile(string filename) {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
                    des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                    ICryptoTransform desEncrypt = des.CreateEncryptor();
                    using (CryptoStream cryptostream = new CryptoStream(stream, desEncrypt, CryptoStreamMode.Write)) {
                        byte[] bytearrayinput = new byte[stream.Length];
                        stream.Read(bytearrayinput, 0, bytearrayinput.Length);
                        stream.SetLength(0);// Clear content                    
                        cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);// Write encrypted content
                    }
                }
            }
        }

        /// Decrypts given file using DES cryptography
        public static void DecryptFile(string filename) {
            using (var DES = new DESCryptoServiceProvider()) {
                DES.Key = Encoding.ASCII.GetBytes(key);
                DES.IV = Encoding.ASCII.GetBytes(key);
                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                using (var ms = new MemoryStream()) {
                    using (var fsread = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                        using (var cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read)) {
                            int data;
                            fsread.Flush();
                            while ((data = cryptostreamDecr.ReadByte()) != -1) {
                                ms.WriteByte((byte)data);
                            }
                            cryptostreamDecr.Close();
                        }
                        using (var fsWrite = new FileStream(filename, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                            ms.WriteTo(fsWrite);
                            ms.Flush();
                        }
                    }
                }
            }
        }
    }
}
