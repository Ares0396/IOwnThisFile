using Main.Support_Tools;
using Main.Support_Tools.Memory_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AIO.Support_Tools.Data_Processing.FileStructure
{
    public class FileStructure32 : IDisposable
    {
        //For internal handling
        private bool _disposed = false;
        private bool _isAnalyzed = false;
        public enum ProcessingMode : int
        {
            Encrypt,
            Decrypt
        }

        private readonly FileStream fs;
        private readonly BinaryReader br;
        private readonly BinaryWriter bw;
        private readonly UnmanagedMemory<byte> fileMemory;
        private ProcessingMode processingMode; //To identify whether we're encrypting or decrypting

        //Use BinaryReader and BinaryWriter
        private string header; //Write raw data, not via BinaryWriter       9 bytes
        private CryptographicOperation.CryptographicMode cryptoMode; //     4 bytes (int as enum)
        private bool isMACEnabled; //                                       1 byte (bool)
        private uint keySize; //                                            4 bytes
        private uint nonceSize; //                                          4 bytes
        private uint saltSize; //                                           4 bytes
        private uint MACSize; //                                            4 bytes
        //                                                               => 30 bytes in total    

        //This the strictly required file structure of an encrypted file (work with raw bytes)
        private Memory<byte> headerRegion;
        private Memory<byte> keyRegion;
        private Memory<byte> nonceRegion; //Each nonce is read as 12 bytes/block. We can detect how many nonces are there with nonce.Length / 12
        private Memory<byte> saltRegion;
        private Memory<byte> dataRegion;
        private Memory<byte> MACRegion;

        public FileStructure32(string filePath, ProcessingMode mode)
        {
            //Check if this file handler supports the file (32 bits only)
            FileInfo fi = new(filePath);
            long length = fi.Length;
            if (length > int.MaxValue) throw new InvalidOperationException("Cannot handle file larger than ~2GB.");
            int fileLength = (int)length;

            //Allocate memory
            fs = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            br = new BinaryReader(fs);
            bw = new BinaryWriter(fs);
            fileMemory = new(fileLength);
            processingMode = mode;

            //We don't fill the memory here and now. We'll have the work done in AnalyzeAsync().
        }
        public void SwapMode(ProcessingMode newMode)
        {
            processingMode = newMode;
        }
        public void Dispose()
        {
            if (_disposed) return;

            CryptographicOperations.ZeroMemory(fileMemory.GetSpan());

            try
            {
                fs.Dispose();
                br.Dispose();
                bw.Dispose();
                fileMemory.Dispose();
            }
            catch (ObjectDisposedException)
            {
                //Do nothing here-Maybe the objects are already disposed
            }

            //The key, nonce, data and MAC memory come from fileMemory, so they're automatically freed. Therefore, no need to dispose them

            _disposed = true;
            GC.SuppressFinalize(this);
        }
        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }
        private void ThrowIfInvalidOperation(ProcessingMode intendedMode)
        {
            if (intendedMode != processingMode) throw new InvalidOperationException("This operation isn't supported.");
        }
        private CryptographicOperation.CryptographicMode ConvertToCryptoMode(int intMode)
        {
            return intMode switch
            {
                0 => CryptographicOperation.CryptographicMode.AES_GCM,
                1 => CryptographicOperation.CryptographicMode.AES_CTR,
                2 => CryptographicOperation.CryptographicMode.AES_CBC,
                3 => CryptographicOperation.CryptographicMode.AES_CFB,
                4 => CryptographicOperation.CryptographicMode.AES_CTS,
                5 => CryptographicOperation.CryptographicMode.AES_ECB,
                _ => throw new InvalidOperationException("Unsupported cryptographic mode is selected.")
            };
        }
        private int ConvertFromCryptoMode(CryptographicOperation.CryptographicMode cryptoMode)
        {
            return cryptoMode switch
            {
                CryptographicOperation.CryptographicMode.AES_GCM => 0,
                CryptographicOperation.CryptographicMode.AES_CTR => 1,
                CryptographicOperation.CryptographicMode.AES_CBC => 2,
                CryptographicOperation.CryptographicMode.AES_CFB => 3,
                CryptographicOperation.CryptographicMode.AES_CTS => 4,
                CryptographicOperation.CryptographicMode.AES_ECB => 5,
                _ => throw new InvalidOperationException("Unsupported cryptographic mode is selected.")
            };
        }

        public async Task AnalyzeAsync()
        {
            //WARNING: FOR DECRYPTION ONLY!!!
            ThrowIfDisposed();
            ThrowIfInvalidOperation(ProcessingMode.Decrypt); //For decryption only
            if (_isAnalyzed) return; //Don't throw. Instead, stop early

            //Let's call some reusable variables
            Memory<byte> data = fileMemory.Memory;

            //Read the file into the memory
            await fs.ReadExactlyAsync(data); //Not ReadAsync, because then it won't guarantee full read

            //Derive the header and the data region (we need to work with the header first, because otherwise, where do we get the others? The metadata is in here)
            headerRegion = data.Slice(0, 26); //The first 26 bytes are the header region
            dataRegion = data.Slice(26, (int)fs.Length - 26); //Safe to cast, as we have that file handler check upon initialization

            header = Encoding.UTF8.GetString(data.Slice(0, 9).Span); //The first 9 bytes are the header string
            if (header != Config.AAD_Header) throw new InvalidDataException("Header mismatch! Likely not encrypted by IOTF tool"); //Validate the file

            //Proceed to parse other info
            fs.Seek(9, SeekOrigin.Begin); //We set the fs offset so that the br doesn't accidentally read the header
            cryptoMode = ConvertToCryptoMode(br.ReadInt32()); //9 + 4 = 13 bytes so far
            isMACEnabled = br.ReadBoolean(); //13 + 1 = 14 bytes so far
            keySize = br.ReadUInt32(); //14 + 4 = 18 bytes so far
            nonceSize = br.ReadUInt32(); //18 + 4 = 22 bytes so far
            saltSize = br.ReadUInt32(); //22 + 4 = 26 bytes so far
            MACSize = br.ReadUInt32(); //26 + 4 = 30 bytes so far => Header processing complete!

            //Now we use the information we obtained to get the nonce, salt, key and stuff!
            //Let's start with the key, since it's the first on our list

            //Later (still the opening code)
        }
    }
}
