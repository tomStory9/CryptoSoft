using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CryptoSoft.Models;
namespace CryptoSoft.Services
{
    class EncryptData
    {
        public CryptoSoftConfig Config { get; set; }

        public EncryptData(CryptoSoftConfig config) {
            Config = config;
        }

        public (string EncryptedFile,byte[] buffer, int bytesRead) EncryptFile(string filePath)
        {
            using FileStream inputFileStream = new FileStream(filePath, FileMode.Open);
            string encryptedfile = filePath + "_encrypted"+".txt";
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

            return (encryptedfile, buffer,bytesRead);
        }


    }
}
