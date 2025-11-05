using Main.Support_Tools.Memory_Management;
using System.Buffers;
using System.Security.Cryptography;

namespace Main.Support_Tools
{
    public static class CryptographicOperation
    {
        public enum CryptographicMode : int
        {
            AES_GCM,
            AES_CTR,
            AES_CBC,
            AES_CFB,
            AES_CTS,
            AES_ECB
        }

        //File processing with custom settings
        public static async Task ProcessFilesAsync(List<string> files, string password, int maxParallelTasks, Config.CryptographyMode mode, IProgress<(string file, bool success)> generalProgress, IProgress<string> fileProgress, bool reuse)
        {
            ParallelOptions options = new() { MaxDegreeOfParallelism = maxParallelTasks };
            AES aes = new(password);
            UnmanagedMemory<byte> mem = null;

            if (reuse)
            {
                int maxSize = 0;
                foreach (string file in files)
                {
                    FileInfo fileInfo = new(file);
                    long fileSize = fileInfo.Length;
                    if (fileSize > maxSize) maxSize = (int)fileSize;
                }
                if (maxSize == 0) throw new InvalidOperationException("Zero size detected - There's possibly no file to encrypt/decrypt.");
                mem = new(maxSize);
            }

            await Parallel.ForEachAsync(files, options, async (file, token) =>
            {
                bool success = true; 
                fileProgress.Report(file);

                try
                {
                    if (new FileInfo(file).Length > int.MaxValue || new FileInfo(file).Length == 0)
                    {
                        success = false;
                        generalProgress.Report((file, success));
                        return;
                    }

                    if (reuse)
                    {
                        if (mode == Config.CryptographyMode.Encrypt)
                        {
                            await aes.EncryptDataAsync(file, mem);
                        }
                        else
                        {
                            await aes.DecryptDataAsync(file, mem);
                        }
                    }
                    else
                    {
                        if (mode == Config.CryptographyMode.Encrypt)
                        {
                            await aes.EncryptDataAsync(file);
                        }
                        else
                        {
                            await aes.DecryptDataAsync(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Handle(ex, () => success = false);
                }

                generalProgress.Report((file, success));
            });
            mem?.Dispose();
            aes.Dispose();
        }


        //All parallel threads called for actions
        public static async Task ProcessFilesAsync(List<string> files, string password, Config.CryptographyMode mode, IProgress<(string file, bool success)> progress, IProgress<string> fileProgress, bool reuse)
        {
            await ProcessFilesAsync(files, password, -1, mode, progress, fileProgress, reuse);
        }
        public static string ComputeHash(byte[] data, HashAlgorithm hasher)
        {
            string stringHash = null!;
            try
            {
                byte[] localHash = hasher.ComputeHash(data);
                stringHash = FormatHash(localHash);
            }
            catch (Exception ex)
            {
                Debugger.Handle(ex, () =>
                {
                    stringHash = string.Empty;
                });
            }
            return stringHash;
        }
        public static async Task<string> ComputeHashAsync(Stream stream, IncrementalHash hasher, CancellationToken token, int chunkSize = 65536)
        {
            ArrayPool<byte> Pool = ArrayPool<byte>.Shared; //Define pooling provider
            byte[] chunkBuffer = null!;
            bool bufferRented = false;
            string hash = null!;

            try
            {
                if (chunkSize >= 1024)
                {
                    chunkBuffer = Pool.Rent(chunkSize); //Use buffer pooling for >1KB bytes
                    bufferRented = true; //Make sure the cleanup mechanism knows it's rented
                }
                else
                {
                    chunkBuffer = new byte[chunkSize]; //Use allocation
                    //No need for setting the flag to false, as it's already defined as false above
                }

                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin); //Reset position

                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(chunkBuffer.AsMemory(0, chunkBuffer.Length), token)) > 0)
                {
                    hasher.AppendData(chunkBuffer, 0, bytesRead); //Feed the hasher chunk by chunk
                }

                //Get final hash and return the equivalent string
                byte[] hashBytes = hasher.GetHashAndReset();
                hash = FormatHash(hashBytes);
            }
            catch (Exception ex)
            {
                Debugger.Handle(ex, () =>
                {
                    hash = string.Empty;
                });
            }
            finally
            {
                if (bufferRented) //If it's rented
                {
                    Pool.Return(chunkBuffer, true);
                }
            }
            return hash;
        }
        public static string FormatHash(byte[] hash)
        {
            if (hash == null || hash == Array.Empty<byte>())
            {
                return string.Empty;
                throw new ArgumentNullException("Hash must not be empty");
            }
            string stringHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return stringHash;
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
        public static async Task GenerateRandomBytesAsync(long length, Stream output, int chunkSize = 8192)
        {
            byte[] buffer = new byte[chunkSize];
            long bytesRemaining = length;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                while (bytesRemaining > 0)
                {
                    int bytesToWrite = (int)Math.Min(chunkSize, bytesRemaining);
                    rng.GetBytes(buffer, 0, bytesToWrite);
                    await output.WriteAsync(buffer.AsMemory(0, bytesToWrite));
                    bytesRemaining -= bytesToWrite;
                }
            }
        }
    }
}
