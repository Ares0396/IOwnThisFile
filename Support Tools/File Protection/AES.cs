using Main.Support_Tools.Memory_Management;
using System.Security.Cryptography;
using System.Text;

namespace Main.Support_Tools
{
    public class AES
    {
        // Change the declaration of 'data' to nullable to fix CS8618
        private readonly string key;
        public AES(string key)
        {
            this.key = key;
        }

        public async Task EncryptDataAsync(string file)
        {
            FileInfo fileInfo = new(file);
            long fileSize = fileInfo.Length;

            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;
            byte[] AAD;
            byte[] tag = new byte[Config.TagSize];
            byte[] headerBytes = Encoding.UTF8.GetBytes(Config.AAD_Header);

            // Read file into rented buffer
            using (FileStream fs = new(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            using (UnmanagedMemory<byte> mem = new((int)fileSize))
            {
                Memory<byte> data = mem.Memory;

                int bytesRead = await fs.ReadAsync(data);
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
                    EncryptDataInternal(aes, nonce, data, data, tag, AAD);
                }

                // Reset file and write
                fs.Position = 0;
                fs.SetLength(0);
                await fs.WriteAsync(AAD);
                await fs.WriteAsync(data);
                await fs.WriteAsync(tag);
                await fs.FlushAsync();

                CryptographicOperations.ZeroMemory(data.Span);
                CryptographicOperations.ZeroMemory(keyBytes);
                CryptographicOperations.ZeroMemory(userKeyBytes);
                CryptographicOperations.ZeroMemory(AAD);
                CryptographicOperations.ZeroMemory(tag);
            }
        }
        public async Task DecryptDataAsync(string file)
        {
            FileInfo fileInfo = new FileInfo(file);

            byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes;
            byte[] AAD;
            byte[] header;
            byte[] nonce;
            byte[] salt;
            byte[] tag;
            int bytesRead;

            // Read file into rented buffer
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            using (UnmanagedMemory<byte> mem = new((int)fileInfo.Length))
            {
                Memory<byte> data = mem.Memory;

                // Async file read
                bytesRead = await fs.ReadAsync(data);
                if (bytesRead != fileInfo.Length)
                    throw new IOException("File read incomplete.");

                // Extract header, nonce, salt, AAD, tag
                header = data.Slice(0, Config.AAD_Header.Length).ToArray();
                nonce = data.Slice(Config.AAD_Header.Length, Config.NonceCount).ToArray();
                salt = data.Slice(Config.AAD_Header.Length + Config.NonceCount, Config.SaltCount).ToArray();
                AAD = data.Slice(0, Config.AAD_Length).ToArray();
                tag = data.Slice(bytesRead - Config.TagSize, Config.TagSize).ToArray();

                var ciphertextLength = bytesRead - Config.AAD_Length - Config.TagSize;
                var ciphertextMemory = data.Slice(Config.AAD_Length, ciphertextLength);

                // Validate header
                string headerString = Encoding.UTF8.GetString(header);
                if (headerString != Config.AAD_Header)
                    throw new CryptographicUnexpectedOperationException("Invalid file header.");

                // Derive key
                using var rfc = new Rfc2898DeriveBytes(userKeyBytes, salt, Config.Iteration, Config.RFC_Algorithm);
                keyBytes = rfc.GetBytes(Config.KeySize);

                // Decrypt (synchronous local span)
                using (AesGcm AES = new(keyBytes, Config.TagSize))
                {
                    DecryptDataInternal(AES, nonce, ciphertextMemory, data.Slice(0, ciphertextLength),  tag, AAD);
                }

                // Overwrite file
                fs.Position = 0;
                fs.SetLength(0);
                await fs.WriteAsync(data.Slice(0, ciphertextLength));
                await fs.FlushAsync();

                // Zero sensitive memory
                CryptographicOperations.ZeroMemory(data.Span);
                CryptographicOperations.ZeroMemory(keyBytes);
                CryptographicOperations.ZeroMemory(userKeyBytes);
                CryptographicOperations.ZeroMemory(AAD);
                CryptographicOperations.ZeroMemory(tag);
            }
        }

        private void EncryptDataInternal(AesGcm aes, byte[] nonce, Memory<byte> data, Memory<byte> encryptedData, byte[] tag, byte[] AAD)
        {
            Span<byte> dataSpan = data.Span;
            Span<byte> encryptedDataSpan = encryptedData.Span;
            aes.Encrypt(nonce.AsSpan(), dataSpan, encryptedDataSpan, tag.AsSpan(), AAD.AsSpan());
        }
        private void DecryptDataInternal(AesGcm aes, byte[] nonce, Memory<byte> encryptedData, Memory<byte> data, byte[] tag, byte[] AAD)
        {
            Span<byte> dataSpan = data.Span;
            Span<byte> encryptedDataSpan = encryptedData.Span;
            aes.Decrypt(nonce.AsSpan(), encryptedDataSpan, tag.AsSpan(), dataSpan, AAD.AsSpan());
        }
    }
}
