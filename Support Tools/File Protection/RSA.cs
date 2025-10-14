using Main.Support_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AIO.Support_Tools.File_Protection
{
    public class RSA : IDisposable
    {
        private readonly System.Security.Cryptography.RSA rsa;

        public enum KeyType
        {
            KeyPublic,
            Keyprivate
        }
        
        public RSA(int bitSize = 2048)
        {
            //For encryption
            rsa = System.Security.Cryptography.RSA.Create(bitSize);
        }
        public RSA(int bitSize, byte[] privKey)
        {
            //For decryption
            rsa = System.Security.Cryptography.RSA.Create(bitSize);
            rsa.ImportRSAPrivateKey(privKey, out _);
        }
        public void Dispose()
        {
            rsa.Dispose();
            GC.SuppressFinalize(this);
        }

        //Encrypt & Decrypt
        public byte[] Encrypt(byte[] data)
        {
            byte[] encrypted = rsa.Encrypt(data, Config.RSAPadding);
            return encrypted;
        }
        public byte[] Decrypt(byte[] data)
        {
            byte[] decrypted = rsa.Decrypt(data, Config.RSAPadding);
            return decrypted;
        }

        //Key management
        public byte[] ExportKey(KeyType keyType)
        {
            if (keyType == KeyType.KeyPublic)
                return rsa.ExportRSAPublicKey();
            else
                return rsa.ExportRSAPrivateKey();
        }
        public RSAParameters ExportParameters(bool includePrivKey)
        {
            return rsa.ExportParameters(includePrivKey);
        }
        public void ImportKey(byte[] key, KeyType keyType)
        {
            if (keyType == KeyType.KeyPublic)
            {
                rsa.ImportRSAPublicKey(key, out _);
            }
            else
            {
                rsa.ImportRSAPrivateKey(key, out _);
            }
        }
        public void ImportParameters(RSAParameters parameters)
        {
            rsa.ImportParameters(parameters);
        }
    }
}
