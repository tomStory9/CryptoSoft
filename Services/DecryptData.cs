using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using CryptoSoft.Models;

namespace CryptoSoft.Services
{
    class DecryptData
    {
        private CryptoSoftConfig _config;

        public CryptoSoftConfig Config
        {
            get { return _config; }
            set { _config = value; }
        }

        public DecryptData(CryptoSoftConfig config)
        {
            _config = config;
        }

        public string DecryptFile(string filePath)
        {
            string decryptedfile = filePath.Replace(".encrypted", "");
            string mutexName = $"Global\\{Path.GetFileName(filePath)}_mutex";

            using (Mutex mutex = new Mutex(false, mutexName))
            {
                try
                {
                    mutex.WaitOne();

                    using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (FileStream outputFileStream = new FileStream(decryptedfile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // Read the IV from the beginning of the encrypted file
                        byte[] iv = new byte[Config.AesAlg.BlockSize / 8];
                        inputFileStream.Read(iv, 0, iv.Length);

                        // Initialize the CryptoSoftConfig with the read IV
                        Config = new CryptoSoftConfig(BitConverter.ToString(Config.EncryptionKey).Replace("-", ""), iv);

                        using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, Config.AesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            // Read the encrypted file and decrypt it
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                outputFileStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }

            return decryptedfile;
        }
    }
}
