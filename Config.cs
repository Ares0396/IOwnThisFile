using System.Security.Cryptography;

namespace Main
{
    public class Config
    {
        public static Dictionary<string, FileStream> LockStream_Dictionary { get; set; } = [];
        public static string AAD_Header { get; set; } = "IOTF v1.0";
        public static int TagSize = 16;
        public static int Iteration = 100000;
        public static HashAlgorithmName RFC_Algorithm = HashAlgorithmName.SHA256;
        public static int SaltCount = 24;
        public static int NonceCount = 12;
        public static int KeySize = 32; // AES-256
        public static int AAD_Length = AAD_Header.Length + NonceCount + SaltCount;
        public static List<string> SelectedFiles { get; set; } = new List<string>();
        public static string Password { get; set; } = string.Empty;
        public enum CryptographyMode
        {
            Encrypt,
            Decrypt
        }
        public static CryptographyMode CryptoMode { get; set; } = CryptographyMode.Encrypt; //Default mode is Encrypt
        public static bool CryptographicOperationSucceeded { get; set; } = true;

        //Property config
        public static string Property_FilePath { get; set; } = string.Empty; //default is empty
        public static bool Property_FileIsLocked { get; set; } = false; //default is false
        public static Dictionary<string, DateTime> Property_FileLastEncrypted = []; //default is empty dictionary
        public static Dictionary<string, DateTime> Property_FileLastLockstreamed = []; //default is empty dictionary
    }
}
