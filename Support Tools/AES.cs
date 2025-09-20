using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Main.Support_Tools
{
    public class AES
    {
        // Change the declaration of 'data' to nullable to fix CS8618
        private byte[]? data;
        private readonly string key;
        public AES(byte[] data, string key)
        {
            this.data = data;
            this.key = key;
        }
        public AES(string key)
        {
            this.key = key;
        }

        public async Task<byte[]> EncryptDataAsync()
        {
            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;
            byte[] encryptedData = new byte[data!.Length];

            using (MemoryStream ms = new())
            {
                await Task.Run(() =>
                {
                    byte[] salt = CryptographicOperation.GenerateRandomBytes(Config.SaltCount);
                    byte[] nonce = CryptographicOperation.GenerateRandomBytes(Config.NonceCount);
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
                });

                return ms.ToArray();
            }
        }
        public async Task EncryptDataAsync(string file)
        {
            FileInfo fileInfo = new(file);
            long fileSize = fileInfo.Length;
            var pool = ArrayPool<byte>.Shared;

            byte[] data = pool.Rent((int)fileSize);
            byte[] encryptedData = pool.Rent((int)fileSize);

            bool bufferDataRented = true;
            bool bufferEncryptedRented = true;

            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;
            byte[] AAD;
            byte[] tag = new byte[Config.TagSize];
            byte[] headerBytes = Encoding.UTF8.GetBytes(Config.AAD_Header);

            try
            {
                // Read file into rented buffer
                using (FileStream fs = new(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    int bytesRead = await fs.ReadAsync(data.AsMemory(0, (int)fileInfo.Length));
                    if (bytesRead != fileInfo.Length)
                        throw new IOException("File read incomplete.");

                    // Prepare AAD
                    byte[] salt = CryptographicOperation.GenerateRandomBytes(Config.SaltCount);
                    byte[] nonce = CryptographicOperation.GenerateRandomBytes(Config.NonceCount);
                    AAD = new byte[Config.AAD_Length];
                    Buffer.BlockCopy(headerBytes, 0, AAD, 0, headerBytes.Length);
                    Buffer.BlockCopy(nonce, 0, AAD, headerBytes.Length, nonce.Length);
                    Buffer.BlockCopy(salt, 0, AAD, headerBytes.Length + nonce.Length, salt.Length);

                    // Derive key
                    using (var rfc = new Rfc2898DeriveBytes(userKeyBytes, salt, Config.Iteration, Config.RFC_Algorithm))
                    {
                        keyBytes = rfc.GetBytes(Config.KeySize);
                    }

                    // Encrypt
                    using (var aes = new AesGcm(keyBytes, Config.TagSize))
                    {
                        aes.Encrypt(
                            nonce,
                            data.AsSpan(0, (int)fileInfo.Length),
                            encryptedData.AsSpan(0, (int)fileInfo.Length),
                            tag,
                            AAD
                        );
                    }

                    // Reset file and write
                    fs.Position = 0;
                    fs.SetLength(0);
                    await fs.WriteAsync(AAD);
                    await fs.WriteAsync(encryptedData.AsMemory(0, (int)fileInfo.Length));
                    await fs.WriteAsync(tag);
                    await fs.FlushAsync();

                    // Clean sensitive data
                    CryptographicOperations.ZeroMemory(data.AsSpan(0, (int)fileInfo.Length));
                    CryptographicOperations.ZeroMemory(encryptedData.AsSpan(0, (int)fileInfo.Length));
                    CryptographicOperations.ZeroMemory(keyBytes);
                    CryptographicOperations.ZeroMemory(userKeyBytes);
                    CryptographicOperations.ZeroMemory(AAD);
                    CryptographicOperations.ZeroMemory(tag);
                }
            }
            finally
            {
                // Return rented buffers
                if (bufferDataRented)
                    pool.Return(data, clearArray: true);
                if (bufferEncryptedRented)
                    pool.Return(encryptedData, clearArray: true);
            }
        }


        public async Task<byte[]> DecryptDataAsync()
        {
            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;

            byte[] AAD;
            byte[] header;
            byte[] nonce;
            byte[] salt;
            byte[] tag;
            byte[] Ciphertext;
            byte[] decryptedData = Array.Empty<byte>();

            await Task.Run(() =>
            {
                AAD = data![..Config.AAD_Length];
                header = data[..Config.AAD_Header.Length];
                nonce = data[Config.AAD_Header.Length..(Config.AAD_Header.Length + Config.NonceCount)];
                salt = data[(Config.AAD_Header.Length + Config.NonceCount)..(Config.AAD_Header.Length + Config.NonceCount + Config.SaltCount)];
                Ciphertext = data[Config.AAD_Length..^Config.TagSize];
                tag = data[^Config.TagSize..];

                //Check header to ensure it's valid and locally encrypted
                string headerString = Encoding.UTF8.GetString(header);
                if (headerString != Config.AAD_Header)
                {
                    decryptedData = Array.Empty<byte>(); // Invalid header, return empty array
                    throw new CryptographicUnexpectedOperationException("");
                }

                using (Rfc2898DeriveBytes RFC = new(userKeyBytes, salt, Config.Iteration, Config.RFC_Algorithm))
                {
                    keyBytes = RFC.GetBytes(Config.KeySize);
                }

                using (AesGcm AES = new(keyBytes, Config.TagSize))
                {
                    decryptedData = new byte[Ciphertext.Length];
                    AES.Decrypt(nonce, Ciphertext, tag, decryptedData, AAD);
                }

                //Clean up
                CryptographicOperations.ZeroMemory(data); // Clear the encrypted data from memory
                CryptographicOperations.ZeroMemory(keyBytes); // Clear the key bytes from memory
                CryptographicOperations.ZeroMemory(AAD); // Clear the AAD from memory
                CryptographicOperations.ZeroMemory(tag); // Clear the tag from memory

                userKeyBytes = null!;
                keyBytes = null!;
                data = null!;
                AAD = null!;
                tag = null!;
                salt = null!;
                Ciphertext = null!;       
            });
            return decryptedData;
        }
        public async Task DecryptDataAsync(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            var pool = ArrayPool<byte>.Shared;

            byte[] data = pool.Rent((int)fileInfo.Length);
            byte[] decryptedData = pool.Rent((int)fileInfo.Length);

            bool bufferDataRented = true;
            bool bufferDecryptedRented = true;

            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;
            byte[] AAD;
            byte[] header;
            byte[] nonce;
            byte[] salt;
            byte[] tag;
            int bytesRead;

            try
            {
                // Read file into rented buffer
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    bytesRead = await fs.ReadAsync(data.AsMemory(0, (int)fileInfo.Length));
                    if (bytesRead != fileInfo.Length)
                        throw new IOException("File read incomplete.");

                    // Extract components
                    header = data[..Config.AAD_Header.Length];
                    nonce = data[Config.AAD_Header.Length..(Config.AAD_Header.Length + Config.NonceCount)];
                    salt = data[(Config.AAD_Header.Length + Config.NonceCount)..(Config.AAD_Header.Length + Config.NonceCount + Config.SaltCount)];
                    AAD = data[..Config.AAD_Length];
                    tag = data.AsMemory(bytesRead - Config.TagSize, Config.TagSize).ToArray();

                    var ciphertextLength = bytesRead - Config.AAD_Length - Config.TagSize;

                    // Copy ciphertext into decryptedData buffer (will overwrite)
                    var ciphertextMemory = data.AsMemory(Config.AAD_Length, ciphertextLength);

                    // Validate header
                    string headerString = Encoding.UTF8.GetString(header);
                    if (headerString != Config.AAD_Header)
                        throw new CryptographicUnexpectedOperationException("Invalid file header.");

                    // Derive key
                    using (var rfc = new Rfc2898DeriveBytes(userKeyBytes, salt, Config.Iteration, Config.RFC_Algorithm))
                    {
                        keyBytes = rfc.GetBytes(Config.KeySize);
                    }

                    // Decrypt
                    using (var aes = new AesGcm(keyBytes, Config.TagSize))
                    {
                        aes.Decrypt(nonce, ciphertextMemory.Span, tag, decryptedData.AsSpan(0, ciphertextLength), AAD);
                    }

                    // Overwrite file with decrypted content
                    fs.Position = 0;
                    fs.SetLength(0);
                    await fs.WriteAsync(decryptedData.AsMemory(0, ciphertextLength));
                    await fs.FlushAsync();

                    // Clean sensitive data
                    CryptographicOperations.ZeroMemory(data);
                    CryptographicOperations.ZeroMemory(decryptedData.AsSpan(0, ciphertextLength));
                    CryptographicOperations.ZeroMemory(keyBytes);
                    CryptographicOperations.ZeroMemory(userKeyBytes);
                    CryptographicOperations.ZeroMemory(AAD);
                    CryptographicOperations.ZeroMemory(tag);
                }
            }
            finally
            {
                if (bufferDataRented)
                    pool.Return(data, clearArray: true);
                if (bufferDecryptedRented)
                    pool.Return(decryptedData, clearArray: true);
            }
        }
    }
}
