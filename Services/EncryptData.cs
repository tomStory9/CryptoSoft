using System;
using System.IO;
using System.Security.Cryptography;
using CryptoSoft.Models;

namespace CryptoSoft.Services
{
    public class EncryptData
    {
        public CryptoSoftConfig Config { get; set; }

        public EncryptData(CryptoSoftConfig config)
        {
            Config = config;
        }

        public (string EncryptedFile, byte[] buffer, int bytesRead) EncryptFile(string filePath)
        {
            using FileStream inputFileStream = new FileStream(filePath, FileMode.Open);

            // Get the directory, filename without extension, and extension
            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            // Construct the new encrypted file path
            string encryptedfile = Path.Combine(directory, fileNameWithoutExtension + "_encrypted" + extension);

            using FileStream outputFileStream = new FileStream(encryptedfile, FileMode.Create);
            using CryptoStream cryptoStream = new CryptoStream(outputFileStream, Config.AesAlg.CreateEncryptor(), CryptoStreamMode.Write);

            // Read the input file and encrypt it
            int bytesRead;
            byte[] buffer = new byte[4096];
            while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cryptoStream.Write(buffer, 0, bytesRead);
            }
            cryptoStream.Close();

            return (encryptedfile, buffer, bytesRead);
        }
    }
}
