using System.Security.Cryptography;
using System.Text;

namespace Main.Support_Tools
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
                CryptographicOperations.ZeroMemory(data); // Clear the data from memory
                CryptographicOperations.ZeroMemory(encryptedData); // Clear the encrypted data from memory
                CryptographicOperations.ZeroMemory(keyBytes); // Clear the key bytes from memory
                CryptographicOperations.ZeroMemory(userKeyBytes); // Clear the user key bytes from memory
                CryptographicOperations.ZeroMemory(AAD); // Clear the AAD from memory
                CryptographicOperations.ZeroMemory(tag); // Clear the tag from memory

                userKeyBytes = null!;
                keyBytes = null!;
                encryptedData = null!;
                AAD = null!;
                tag = null!;
                salt = null!;
                data = null!;

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

                AAD = encryptedData[..Config.AAD_Length];
                header = encryptedData[..Config.AAD_Header.Length];
                nonce = encryptedData[Config.AAD_Header.Length..(Config.AAD_Header.Length + Config.NonceCount)];
                salt = encryptedData[(Config.AAD_Header.Length + Config.NonceCount)..(Config.AAD_Header.Length + Config.NonceCount + Config.SaltCount)];
                Ciphertext = encryptedData[Config.AAD_Length..^Config.TagSize];
                tag = encryptedData[^Config.TagSize..];

                //Check header to ensure it's valid and locally encrypted
                string headerString = Encoding.UTF8.GetString(header);
                if (headerString != Config.AAD_Header)
                {
                    return decryptedData = []; // Invalid header, return empty array
                    throw new InvalidOperationException(); // Throw an exception so that CryptoOperationSucceeded is set to false
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

                userKeyBytes = null!;
                keyBytes = null!;
                encryptedData = null!;
                AAD = null!;
                tag = null!;
                salt = null!;
                Ciphertext = null!;
                decryptedData = null!;

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
            ArgumentNullException.ThrowIfNull(output);
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
        public static string HashContent(byte[] buffer, HashAlgorithm hasher)
        {
            byte[] localHash = hasher.ComputeHash(buffer);
            string stringHash = BitConverter.ToString(localHash).Replace("-", "").ToLowerInvariant();
            return stringHash;
        }
        public static string ConvertRawBytes(long size)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB" };
            double len = size;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Format with 3 decimal places
            return $"{len:0.###} {sizes[order]}";
        }
    }
}
