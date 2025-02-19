using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CryptoSoft.Models;

namespace CryptoSoft.Services
{
    class DecryptData
    {
        public CryptoSoftConfig Config { get; set; }

        public DecryptData(CryptoSoftConfig config)
        {
            Config = config;
        }

        public string DecryptFile(string filePath)
        {
            string decryptedFile = filePath;
            int bytesRead = 0;
            byte[] buffer = new byte[4096]; // Initialize buffer with a size
            using FileStream encryptedFileStream = new FileStream(filePath, FileMode.Open);
            using FileStream decryptedFileStream = new FileStream(decryptedFile, FileMode.Create);
            using CryptoStream decryptStream = new CryptoStream(encryptedFileStream, Config.AesAlg.CreateDecryptor(), CryptoStreamMode.Read);
            // Decrypt the encrypted file and write the decrypted content
            while ((bytesRead = decryptStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                decryptedFileStream.Write(buffer, 0, bytesRead);
            }
            decryptStream.Close();

            return decryptedFile;
        }
    }
}
