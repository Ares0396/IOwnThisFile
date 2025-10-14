using Main.Support_Tools.Memory_Management;
using System.Security.Cryptography;
using System.Text;

namespace Main.Support_Tools
{
    public class AES
    {
        //TODO: Internally reuse mem

        private bool _disposed;
        private readonly string key;
        private readonly SemaphoreSlim sema = new(1, 1); //Async variation of private obj lock
        public AES(string key)
        {
            this.key = key;
        }
        public void Dispose()
        {
            sema.Dispose();
            _disposed = true;
        }
        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        public async Task EncryptDataAsync(string file)
        {
            //This uses its own mem, so why use sema?

            ThrowIfDisposed();

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

                // Read file into rented buffer
                int bytesRead = 0;
                while (bytesRead < fileSize)
                {
                    int read = await fs.ReadAsync(data[bytesRead..]);
                    if (read == 0) throw new EndOfStreamException("Unexpected EOF.");
                    bytesRead += read;
                }

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
        public async Task EncryptDataAsync(string file, UnmanagedMemory<byte> mem)
        {
            //If a mem is reused across threads, a race condition is inevitable
            //That's why we use SemaphoreSlim

            ThrowIfDisposed();
            await sema.WaitAsync();
            try
            {
                FileInfo fileInfo = new(file);
                long fileSize = fileInfo.Length;

                byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes;
                byte[] AAD;
                byte[] tag = new byte[Config.TagSize];
                byte[] headerBytes = Encoding.UTF8.GetBytes(Config.AAD_Header);

                using FileStream fs = new(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                Memory<byte> data = mem.Memory;

                //Check if the memory size is big enough to contain the encrypted data
                //We'll compute the output based on the size of each structural section
                long estimatedOutputSize = Config.AAD_Header.Length //Header
                                         + Config.NonceCount        //Nonce
                                         + Config.SaltCount         //Salt
                                         + fileSize                 //Original data
                                         + Config.TagSize;          //Tag
                if (estimatedOutputSize > int.MaxValue)
                {
                    //Detected the issue of size being too large - Abort and throw
                    throw new InvalidOperationException("Operation does not support files larger than ~2GB");
                }
                if (data.Length < estimatedOutputSize)
                {
                    //Memory size is lower than actual estimated output! Resize it!
                    mem.Resize((int)estimatedOutputSize); //No support for arrays of long-type size, so let it throw. Users can handle that.
                }

                // Read file into rented buffer
                int bytesRead = 0;
                while (bytesRead < fileSize)
                {
                    int read = await fs.ReadAsync(data[bytesRead..]);
                    if (read == 0) throw new EndOfStreamException("Unexpected EOF.");
                    bytesRead += read;
                }

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

                CryptographicOperations.ZeroMemory(data.Span); //We want to securely clear the data after each operation
                CryptographicOperations.ZeroMemory(keyBytes);
                CryptographicOperations.ZeroMemory(userKeyBytes);
                CryptographicOperations.ZeroMemory(AAD);
                CryptographicOperations.ZeroMemory(tag);

                //NOTE: DON'T DELETE THE UNMANAGED MEMORY - IT WILL BE REUSED INSTEAD!!!
            }
            finally
            {
                sema.Release();
            }
        }

        public async Task DecryptDataAsync(string file)
        {
            //This uses its own mem, so why use sema?

            ThrowIfDisposed();

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
        public async Task DecryptDataAsync(string file, UnmanagedMemory<byte> mem)
        {
            //If a mem is reused across threads, a race condition is inevitable
            //That's why we use SemaphoreSlim

            ThrowIfDisposed();
            await sema.WaitAsync();
            try
            {
                FileInfo fileInfo = new(file);

                byte[] userKeyBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes;
                byte[] AAD;
                byte[] header;
                byte[] nonce;
                byte[] salt;
                byte[] tag;
                int bytesRead;

                // Read file into rented buffer
                using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                Memory<byte> data = mem.Memory;

                //Make sure the input size must not be larger than ~2GB
                //If the check passes, the output will always be smaller
                long inputSize = fileInfo.Length;
                if (inputSize > int.MaxValue)
                    throw new InvalidOperationException("The input file (possibly >2GB) is too large for a supported operation.");

                //Check if the memory size is big enough to contain the output (not input, so subtract some values to get the actual data length)
                int estimatedOutputSize = (int)inputSize             //The input, including those small arrays
                                        - Config.AAD_Header.Length   //The header
                                        - Config.NonceCount          //The nonce
                                        - Config.SaltCount           //The salt
                                        - Config.TagSize;            //The tag
                if (data.Length < estimatedOutputSize) //Safe, because it'll always be an int
                {
                    //The size is too small, resize it
                    mem.Resize(estimatedOutputSize);
                }

                // Async file read
                bytesRead = 0;
                while (bytesRead < inputSize)
                {
                    int read = await fs.ReadAsync(data[bytesRead..]);
                    if (read == 0) throw new EndOfStreamException("Unexpected EOF.");
                    bytesRead += read;
                }

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
                    DecryptDataInternal(AES, nonce, ciphertextMemory, data.Slice(0, ciphertextLength), tag, AAD);
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

                // NOTE: DON'T DELETE THE UNMANAGED MEMORY - IT WILL BE REUSED INSTEAD!!!
            }
            finally
            {
                sema.Release();
            }
        }

        private static void EncryptDataInternal(AesGcm aes, byte[] nonce, Memory<byte> data, Memory<byte> encryptedData, byte[] tag, byte[] AAD)
        {
            Span<byte> dataSpan = data.Span;
            Span<byte> encryptedDataSpan = encryptedData.Span;
            aes.Encrypt(nonce.AsSpan(), dataSpan, encryptedDataSpan, tag.AsSpan(), AAD.AsSpan());
        }
        private static void DecryptDataInternal(AesGcm aes, byte[] nonce, Memory<byte> encryptedData, Memory<byte> data, byte[] tag, byte[] AAD)
        {
            Span<byte> dataSpan = data.Span;
            Span<byte> encryptedDataSpan = encryptedData.Span;
            aes.Decrypt(nonce.AsSpan(), encryptedDataSpan, tag.AsSpan(), dataSpan, AAD.AsSpan());
        }
    }
}
