using System;
using System.IO;
using System.Security.Cryptography;
using CryptoSoft.Models;
using CryptoSoft.Services;

class Program
{

    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: CryptoSoft <command> <inputFile> <outputPath>");
            Console.WriteLine("Commands: encrypt, decrypt");
            return;
        }

        string command = args[0];
        string inputFile = args[1];
        string outputPath = args[2];

        // Initialize the CryptoSoftConfig with AES algorithm
        CryptoSoftConfig config = new CryptoSoftConfig();

        if (command.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
        {
            // Encrypt the file
            EncryptData encryptData = new EncryptData(config);
            var (encryptedFile, buffer, bytesRead) = encryptData.EncryptFile(inputFile);
            string encryptedFilePath = Path.Combine(outputPath, Path.GetFileName(encryptedFile));
            File.Move(encryptedFile, encryptedFilePath);
            Console.WriteLine($"File encrypted to: {encryptedFilePath}");
        }
        else if (command.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
        {
            // Decrypt the file
            DecryptData decryptData = new DecryptData(config);
            string decryptedFile = decryptData.DecryptFile(inputFile);
            string decryptedFilePath = Path.Combine(outputPath, decryptedFile);
            File.Move(decryptedFile, decryptedFilePath);
            Console.WriteLine($"File decrypted to: {decryptedFilePath}");
        }
        else
        {
            Console.WriteLine("Invalid command. Use 'encrypt' or 'decrypt'.");
        }
    }
}
