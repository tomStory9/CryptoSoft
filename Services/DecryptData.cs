using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CryptoSoft.Models;

namespace CryptoSoft.Services
{
    public class DecryptData
    {
        public CryptoSoftConfig Config { get; set; }

        public DecryptData(CryptoSoftConfig config)
        {
            Config = config;
        }

        public string DecryptFile(string filePath)
        {
            // Ensure the output file has "_decrypted" suffix to avoid overwriting
            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            string decryptedFile = Path.Combine(directory, fileNameWithoutExtension + "_decrypted" + extension);

            byte[] buffer = new byte[4096]; // Buffer size for reading
            int bytesRead;

            using FileStream encryptedFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using FileStream decryptedFileStream = new FileStream(decryptedFile, FileMode.Create, FileAccess.Write, FileShare.None);
            using CryptoStream decryptStream = new CryptoStream(encryptedFileStream, Config.AesAlg.CreateDecryptor(), CryptoStreamMode.Read);

            // Read from the encrypted stream and write to the decrypted file
            while ((bytesRead = decryptStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                decryptedFileStream.Write(buffer, 0, bytesRead);
            }

            return decryptedFile; // Return the new decrypted file path
        }

    }
}
