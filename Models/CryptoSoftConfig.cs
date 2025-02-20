using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CryptoSoft.Models
{
        public class CryptoSoftConfig
    {
        public  Aes AesAlg ;
        public  byte[] EncryptionKey;
        public  byte[] IV;
        

       public CryptoSoftConfig()
        {
            AesAlg = Aes.Create();
            EncryptionKey = AesAlg.Key;
            IV = AesAlg.IV;
        }
    }

}
