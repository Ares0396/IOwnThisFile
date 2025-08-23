using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Main
{
    public class Tool
    {
        public static FileStream InitializeLockStream(string filePath)
        {
            FileStream lockStream;
            try
            {
                // Create a FileStream with exclusive access to the file
                lockStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                return lockStream;
            }
            catch (IOException ex)
            {
                // Handle the case where the file is already locked or inaccessible
                Console.WriteLine($"Error initializing lock stream: {ex.Message}");
                return null;
            }
        }
        public static byte[] EncryptData(byte[] data, string key)
        {
            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;
            byte[] encryptedData = new byte[data.Length];

            using (MemoryStream ms = new())
            {
                byte[] salt = GenerateRandomBytes(Config.SaltCount);
                byte[] nonce = GenerateRandomBytes(Config.NonceCount);
                byte[] tag = new byte[Config.TagSize];
                byte[] AAD = new byte[Config.AAD_Length];
                byte[] headerBytes = Encoding.UTF8.GetBytes(Config.AAD_Header);
                
                Buffer.BlockCopy(headerBytes, 0, AAD, 0, headerBytes.Length);
                Buffer.BlockCopy(nonce, 0, AAD, headerBytes.Length, nonce.Length);
                Buffer.BlockCopy(salt, 0, AAD, headerBytes.Length + nonce.Length, salt.Length);

                using (Rfc2898DeriveBytes RFC = new(userKeyBytes, salt, Config.Iteration, Config.RFC_Algorithm))
                {
                    keyBytes = RFC.GetBytes(Config.KeySize);
                }

                using (AesGcm AES = new(keyBytes, Config.TagSize))
                {
                    AES.Encrypt(nonce, data, encryptedData, tag, AAD);
                }

                ms.Write(AAD, 0, AAD.Length);
                ms.Write(encryptedData, 0, encryptedData.Length);
                ms.Write(tag, 0, tag.Length);

                //Clean up
                CryptographicOperations.ZeroMemory(data); // Clear the original data from memory
                CryptographicOperations.ZeroMemory(encryptedData); // Clear the encrypted data from memory
                CryptographicOperations.ZeroMemory(keyBytes); // Clear the key bytes from memory
                CryptographicOperations.ZeroMemory(AAD); // Clear the AAD from memory
                CryptographicOperations.ZeroMemory(tag); // Clear the tag from memory

                return ms.ToArray();
            }
        }
        public static byte[] DecryptData(byte[] encryptedData, string key)
        {
            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;     
            
            using (MemoryStream ms = new())
            {
                byte[] AAD;
                byte[] header;
                byte[] nonce;
                byte[] salt;
                byte[] tag;
                byte[] Ciphertext;
                byte[] decryptedData;

                using (MemoryStream ms_AAD = new())
                {
                    ms_AAD.Write(encryptedData, 0, Config.AAD_Length);
                    AAD = ms_AAD.ToArray();
                }
                using (MemoryStream ms_Header = new())
                {
                    ms_Header.Write(encryptedData, 0, Config.AAD_Header.Length);
                    header = ms_Header.ToArray();
                }
                using (MemoryStream ms_Nonce = new())
                {
                    ms_Nonce.Write(encryptedData, Config.AAD_Header.Length, Config.NonceCount);
                    nonce = ms_Nonce.ToArray();
                }
                using (MemoryStream ms_Salt = new())
                {
                    ms_Salt.Write(encryptedData, Config.AAD_Header.Length + Config.NonceCount, Config.SaltCount);
                    salt = ms_Salt.ToArray();
                }
                using (MemoryStream ms_Ciphertext = new())
                {
                    ms_Ciphertext.Write(encryptedData, Config.AAD_Length, encryptedData.Length - Config.AAD_Length - Config.TagSize);
                    Ciphertext = ms_Ciphertext.ToArray();
                }
                using (MemoryStream ms_Tag = new())
                {
                    ms_Tag.Write(encryptedData, encryptedData.Length - Config.TagSize, Config.TagSize);
                    tag = ms_Tag.ToArray();
                }

                using (Rfc2898DeriveBytes RFC = new(userKeyBytes, salt, Config.Iteration, Config.RFC_Algorithm))
                {
                    keyBytes = RFC.GetBytes(Config.KeySize);
                }

                using (AesGcm AES = new(keyBytes, Config.TagSize))
                {
                    decryptedData = new byte[Ciphertext.Length];
                    AES.Decrypt(nonce, Ciphertext, tag, decryptedData, AAD);
                    ms.Write(decryptedData, 0, decryptedData.Length);
                }

                //Clean up
                CryptographicOperations.ZeroMemory(decryptedData); // Clear the decrypted data from memory
                CryptographicOperations.ZeroMemory(encryptedData); // Clear the encrypted data from memory
                CryptographicOperations.ZeroMemory(keyBytes); // Clear the key bytes from memory
                CryptographicOperations.ZeroMemory(AAD); // Clear the AAD from memory
                CryptographicOperations.ZeroMemory(tag); // Clear the tag from memory

                return ms.ToArray();
            }
        }
        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }
        public static void GenerateRandomBytes(long length, Stream output, int chunkSize = 8192)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (!output.CanWrite) throw new ArgumentException("Stream must be writable.", nameof(output));

            byte[] buffer = new byte[chunkSize];
            long bytesRemaining = length;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                while (bytesRemaining > 0)
                {
                    int bytesToWrite = (int)Math.Min(chunkSize, bytesRemaining);
                    rng.GetBytes(buffer, 0, bytesToWrite);
                    output.Write(buffer, 0, bytesToWrite);
                    bytesRemaining -= bytesToWrite;
                }
            }
        } //AI-generated. Will make this commercial later.
        public static void WriteDataByChunk(byte[] data, Stream output, int chunkSize = 8192)
        {
            //Check
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (!output.CanWrite) throw new ArgumentException("Stream must be writable.", nameof(output));

            //Define variables
            byte[] buffer = new byte[chunkSize];
            int offset = 0;

            //Write data by chunk
            while (offset < data.Length)
            {
                int bytesToWrite = Math.Min(chunkSize, data.Length - offset);
                Array.Copy(data, offset, buffer, 0, bytesToWrite);
                output.Write(buffer, 0, bytesToWrite);
                offset += bytesToWrite;
            }
        }
    }
}
