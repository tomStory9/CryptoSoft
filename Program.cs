using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using CryptoSoft.Models;
using CryptoSoft.Services;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: CryptoSoft <command> <inputFile> <outputPath> <key>");
            Console.WriteLine("Commands: encrypt, decrypt");
            return;
        }

        string command = args[0];
        string inputFile = args[1];
        string outputPath = args[2];
        string key = args[3];


        // Initialize the CryptoSoftConfig with the generated key
        CryptoSoftConfig config = new CryptoSoftConfig(key);

        if (command.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
        {
            // Encrypt the file
            EncryptData encryptData = new EncryptData(config);
            var result = encryptData.EncryptFile(inputFile);
            string encryptedFilePath = Path.Combine(outputPath, Path.GetFileName(result.EncryptedFile));
            File.Move(result.EncryptedFile, encryptedFilePath);
            Console.WriteLine($"File encrypted to: {encryptedFilePath}");
        }
        else if (command.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
        {
            // Ensure the same key and IV are used for decryption
            DecryptData decryptData = new DecryptData(config);
            string decryptedFile = decryptData.DecryptFile(inputFile);
            string decryptedFilePath = Path.Combine(outputPath, Path.GetFileName(decryptedFile));
            File.Move(decryptedFile, decryptedFilePath);
            Console.WriteLine($"File decrypted to: {decryptedFilePath}");
        }
        else
        {
            Console.WriteLine("Invalid command. Use 'encrypt' or 'decrypt'.");
        }
    }
}
