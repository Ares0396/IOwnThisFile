using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text.Json;

namespace Main.Support_Tools
{
    public class Config //Config is only used for internal/dev purposes
    {
        public static Dictionary<string, FileStream> LockStream_Dictionary { get; set; } = []; //LockStream dictionary

        //Cryptography config for AES-GCM
        public static string AAD_Header { get; set; } = "IOTF v1.0";
        public static readonly int TagSize = 16;
        public static readonly int Iteration = 500000;
        public static readonly HashAlgorithmName RFC_Algorithm = HashAlgorithmName.SHA256;
        public static readonly int SaltCount = 24;
        public static readonly int NonceCount = 12;
        public static readonly int KeySize = 32; // AES-256
        public static readonly int AAD_Length = AAD_Header.Length + NonceCount + SaltCount;
        public static List<string> SelectedFiles { get; set; } = [];
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

        //Update checker config
        public static string UpdateChecker_URL { get; set; } = "https://raw.githubusercontent.com/Ares0396/IOwnThisFile/refs/heads/main/VersionInfo.txt"; //IMPORTANT! DO NOT CHANGE!!!
        private static readonly string UpdateChecker_TempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string UpdateChecker_TempFilename = "iotftemp.exe";
        public static readonly string UpdateChecker_TempFilePath = Path.Combine(UpdateChecker_TempFolder, UpdateChecker_TempFilename);
        public static string[] versionInfoLines { get; set; } = [];
        public static string latestVersion_String { get; set; } = string.Empty;
        public static Version latestVersion { get; set; } = new();
        public static Version currentVersion { get; set; } = new();
        public enum UpdateMode
        {
            Phase1, Phase2, Phase3
        }//phase 1 = get update; phase 2 = replace; phase 3 = cleanup
        public static UpdateMode updateMode { get; set; } = UpdateMode.Phase1; //Default
        public static string ExePath { get; set; } = Application.ExecutablePath;

        //Startup command config
        public static readonly string Command_UpdatePhase2 = "--update2"; //--update PathTo
        public static readonly string Command_UpdatePhase3 = "--update3";
        public static readonly string Command_GetExeHash = "--hash";

        //AppSetting config
        public static readonly JsonSerializerOptions CachedJsonOptions = new() { WriteIndented = true };
        public static bool SettingsChanged { get; set; } = false; //Flag to indicate if settings have been changed since last save
        public static readonly string Reg_AppSettingPath = @"Software\IOwnThisFile"; //Registry path for app settings, in CURRENT_USER (key)
        public static readonly string Reg_AppSetting_FilePath = "SettingPath"; //Registry name for app settings (value)
        public static readonly string Reg_RetryMaxCount = "RetryMaxCount"; //Registry name for retry max count (value)
        public static readonly string Reg_AutoCheck = "AutoCheck"; //Registry name for auto check (value)
        public static readonly string Reg_Theme = "AppTheme"; //Registry name for app theme (value)
        public static readonly string Reg_IOLockList = "IOLockListPath"; //Registry name for IO lock list (value) (path to file)
        public static readonly string Reg_SmartWriteSelectorThreshold = "SmartWriteSelectorThreshold"; //Registry name for Smart Write Selector threshold (value)
        public static readonly string Reg_SmartWriteSelectorAllOutMode = "SmartWriteSelectorAllOutMode"; //Registry name for Smart Write Selector all-out mode (value)
        public static readonly string Reg_SettingSaveMode = "SettingSaveMode"; //Registry name for setting save mode (string value)

        //Color config (For Dark Mode)
        public static readonly Color Color_Background = Color.FromArgb(45, 45, 48);   // Dark gray form background
        public static readonly Color Color_ControlBackground = Color.FromArgb(28, 28, 28);
        public static readonly Color Color_ButtonBackground = Color.FromArgb(63, 63, 70);
        public static readonly Color Color_TextColor = Color.Gainsboro;               // Soft white text
        public static readonly Color Color_Highlight = Color.FromArgb(0, 122, 204);   // Accent color (blue)

        //Color config (For Light Mode)
        public static readonly Color Color_LightBackground = Color.FromArgb(240, 240, 240); // Light gray form background
        public static readonly Color Color_LightControlBackground = Color.FromArgb(255, 255, 255);
        public static readonly Color Color_LightButtonBackground = Color.FromArgb(221, 221, 221);
        public static readonly Color Color_LightTextColor = Color.FromArgb(30, 30, 30);     // Dark text
        public static readonly Color Color_LightHighlight = Color.FromArgb(0, 122, 204);    // Accent color (blue)

        public static RegistryKey GetRegKey()
        {
            var key = Registry.CurrentUser.OpenSubKey(Reg_AppSettingPath, true);
            return key ?? Registry.CurrentUser.CreateSubKey(Reg_AppSettingPath);
        }
    }
}
