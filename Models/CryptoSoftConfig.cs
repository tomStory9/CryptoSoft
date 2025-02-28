using System;
using System.Security.Cryptography;

namespace CryptoSoft.Models
{
    public class CryptoSoftConfig
    {
        public Aes AesAlg { get; private set; }
        public byte[] EncryptionKey { get; private set; }
        public byte[] IV { get; private set; }

        public CryptoSoftConfig(string hex)
        {
            if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have an even length and not be null or empty.", nameof(hex));
            }

            // Convert the hex string to a byte array
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            // Validate the key length
            if (bytes.Length != 16 && bytes.Length != 24 && bytes.Length != 32)
            {
                throw new ArgumentException("Key must be 128, 192, or 256 bits long.", nameof(hex));
            }

            // Initialize AES algorithm
            AesAlg = Aes.Create();
            EncryptionKey = new byte[bytes.Length];
            Array.Copy(bytes, EncryptionKey, bytes.Length);
            AesAlg.Key = EncryptionKey;

            // Generate a new IV
            AesAlg.GenerateIV();
            IV = AesAlg.IV;
        }

        public CryptoSoftConfig(string hex, byte[] iv) : this(hex)
        {
            // Use the provided IV
            IV = iv;
            AesAlg.IV = IV;
        }
    }
}
