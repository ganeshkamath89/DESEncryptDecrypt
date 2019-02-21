using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionDecryptionTestDES
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length >= 1)
            {
                string text = args[0].Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    string encryptedText = EncryptDecrypt.Encrypt(text);

                    Console.WriteLine("Encrypted Text: " + encryptedText);
                    string decryptedText = EncryptDecrypt.Decrypt(encryptedText);
                    Console.WriteLine("Decrypted Text: " + decryptedText);
                }
                else
                {
                    Console.WriteLine("Pass input without whitespaces.");
                }
            }
            else
            {
                Console.WriteLine("This program only accepts one argument.");
            }
        }
    }
}