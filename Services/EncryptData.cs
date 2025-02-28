using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using CryptoSoft.Models;

namespace CryptoSoft.Services
{
    class EncryptData
    {
        public CryptoSoftConfig Config { get; set; }

        public EncryptData(CryptoSoftConfig config)
        {
            Config = config;
        }

        public string EncryptFile(string filePath)
        {
            string encryptedfile = filePath + ".encrypted";
            string mutexName = $"Global\\{Path.GetFileName(filePath)}_mutex";

            using (Mutex mutex = new Mutex(false, mutexName))
            {
                try
                {
                    mutex.WaitOne();

                    using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (FileStream outputFileStream = new FileStream(encryptedfile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // Write the IV at the beginning of the encrypted file
                        outputFileStream.Write(Config.IV, 0, Config.IV.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, Config.AesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            // Read the input file and encrypt it
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, bytesRead);
                            }
                            cryptoStream.FlushFinalBlock();
                        }
                    }
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }

            return encryptedfile;
        }
    }
}
