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
        var appName = Assembly.GetEntryAssembly().GetName().Name;
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: CryptoSoft <command> <inputFile> <outputPath> <key>");
            Console.WriteLine("Commands: encrypt, decrypt");
            Console.WriteLine(appName);
            return;
        }

        string command = args[0];
        string inputFile = args[1];
        string outputPath = args[2];
        string key = args[3];

        // Initialize the CryptoSoftConfig with the provided key
        CryptoSoftConfig config = new CryptoSoftConfig(key);

        try
        {
            if (command.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
            {
                // Encrypt the file
                EncryptData encryptData = new EncryptData(config);
                string encryptedFilePath = encryptData.EncryptFile(inputFile);
                string finalEncryptedFilePath = Path.Combine(outputPath, Path.GetFileName(encryptedFilePath));
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                File.Move(encryptedFilePath, finalEncryptedFilePath);
                Console.WriteLine($"File encrypted to: {finalEncryptedFilePath}");
            }
            else if (command.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
            {
                // Decrypt the file
                DecryptData decryptData = new DecryptData(config);
                string decryptedFilePath = decryptData.DecryptFile(inputFile);
                string finalDecryptedFilePath = Path.Combine(outputPath, Path.GetFileName(decryptedFilePath));
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                File.Move(decryptedFilePath, finalDecryptedFilePath);
                Console.WriteLine($"File decrypted to: {finalDecryptedFilePath}");
            }
            else
            {
                Console.WriteLine("Invalid command. Use 'encrypt' or 'decrypt'.");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
