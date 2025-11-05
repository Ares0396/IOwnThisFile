using System.Buffers;
using System.Security.Cryptography;

namespace Main.Support_Tools
{
    public class Streamer
    {
        public static async Task WriteDownloadedDataAsync(string URL, Stream stream, IProgress<double> progress, int chunkSize = 65536, HttpClient client = null, int retryMax = 1, CancellationToken token = default)
        {
            await WriteAndVerifyDownloadedDataAsync(URL, stream, progress, chunkSize, client, retryMax, null, token); // = no hashing
        }
        public static async Task<bool> WriteAndVerifyDownloadedDataAsync(string URL, Stream stream, IProgress<double> progress, int chunkSize = 65536, HttpClient client = null, int retryMax = 1, string expectedHash = null, CancellationToken token = default)
        {
            ArrayPool<byte> Pool = ArrayPool<byte>.Shared; //Define pooling provider
            byte[] chunkBuffer = null; //Silence the compiler error
            bool bufferRented = false;
            bool success = false;
            int attemptCount = 0;
            bool DisposeClient = false;
            string generatedHash;

            //Determine if HttpClient is passed or not
            if (client == null)
            {
                client = new HttpClient();
                DisposeClient = true;
            }
            if (retryMax <= 0)
            {
                throw new ArgumentException("The smallest Max retry count is 1.");
            }

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

                while (attemptCount < retryMax && !success) //Retry if not successful AND attemptCount is less than retryMax
                {
                    attemptCount++; //Increment attempt by 1

                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin); //Reset position
                        stream.SetLength(0); //Clear all the data
                    }

                    using (HttpResponseMessage responseMsg = await client.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead, token))
                    using (IncrementalHash hasher = IncrementalHash.CreateHash(Config.RFC_Algorithm))
                    using (Stream remoteStream = responseMsg.Content.ReadAsStream(token))
                    {
                        long? totalBytes = responseMsg.Content.Headers.ContentLength;
                        long bytesReadSoFar = 0;
                        int bytesRead;

                        while ((bytesRead = await remoteStream.ReadAsync(chunkBuffer.AsMemory(0, chunkBuffer.Length), token)) > 0)
                        {
                            await stream.WriteAsync(chunkBuffer.AsMemory(0, bytesRead), token); //Write
                            hasher.AppendData(chunkBuffer, 0, bytesRead); //bytesRead, not chunkBuffer.Length, because we could accidentally read zeroes

                            bytesReadSoFar += bytesRead;

                            if (totalBytes.HasValue)
                            {
                                progress?.Report((double)bytesReadSoFar / totalBytes.Value); //Report
                            }
                        }
                        generatedHash = CryptographicOperation.FormatHash(hasher.GetHashAndReset());
                    }

                    if (expectedHash is null || generatedHash == expectedHash)
                    {
                        success = true; //Mark if the operation is successful
                    }
                    else success = false;
                }

                return success;
            }
            catch (Exception ex) when (attemptCount == retryMax)
            {
                success = false; //Mark if the operation is unsuccessful
                Debugger.Handle(ex);
                return success;
            }
            catch
            {
                success = false; //Mark if the operation is unsuccessful
                return success;
            }
            finally //Clean up
            {
                if (bufferRented && chunkBuffer != null) //If it's rented
                {
                    Pool.Return(chunkBuffer, true);
                }
                if (DisposeClient) client.Dispose(); //Dispose if it's created here
            }
        }
        public static async Task WriteLocalDataAsync(byte[] data, Stream stream, IProgress<double> progress, CancellationToken token, int chunkSize = 65536)
        {
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin); //Reset position
            int offset = 0;

            while (offset < data.Length)
            {
                try
                {
                    int refinedChunkSize = Math.Min(chunkSize, data.Length - offset); //Make sure the remaining bytes to be written doesn't exceed array bound
                    await stream.WriteAsync(data.AsMemory(offset, refinedChunkSize), token); //Just write chunk-by-chunk here
                    offset += refinedChunkSize; //Update offset

                    //Report
                    progress.Report((double)offset / data.Length);
                }
                catch (Exception ex)
                {
                    Debugger.Handle(ex);
                } 
            }
        }
        public static async Task WriteLocalStreamAsync(Stream srcStream, Stream desStream, IProgress<double> progress, int chunkSize = 65536, CancellationToken token = default)
        {
            if (srcStream.CanSeek) srcStream.Seek(0, SeekOrigin.Begin);
            if (desStream.CanSeek) desStream.Seek(0, SeekOrigin.Begin);

            ArrayPool<byte> Pool = ArrayPool<byte>.Shared; //Define pooling provider
            byte[] chunkBuffer = null!;
            bool bufferRented = false;
            int bytesRead;

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

                while ((bytesRead = await srcStream.ReadAsync(chunkBuffer, token)) > 0)
                {
                    await desStream.WriteAsync(chunkBuffer.AsMemory(0, bytesRead), token);
                    progress.Report((double)srcStream.Position / srcStream.Length);
                }
            }
            catch (Exception ex)
            {
                Debugger.Handle(ex);
            }
            finally
            {
                if (bufferRented) //If it's rented
                {
                    Pool.Return(chunkBuffer, true);
                }
            }
        }
    }
}
