using Main;
using Main.Support_Tools;
using System.Security.Cryptography;

namespace AIO.Forms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Initialize app and get ready to apply settings before running
            ApplicationConfiguration.Initialize();
            Debugger.AttachGlobalHandlers(() =>
            {
                MessageBox.Show("A fatal error occurred. Click \"Ok\" to close the program.", "Fatal error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            });

            if (!OperatingSystem.IsWindows())
            {
                //The user's OS is unlikely Windows
                var result = MessageBox.Show("This software is designed for Windows only. Are you sure you want to continue? Features may not work correctly.", "Incompatibility detected!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) Environment.Exit(1);
            }

            //Handle startup commands
            if (args.Length > 0)
            {
                if (args[0] == Config.Command_UpdatePhase2)
                {
                    Config.ExePath = args[1];
                    Config.updateMode = Config.UpdateMode.Phase2;
                    Application.Run(new UpdateForm());
                }
                else if (args[0] == Config.Command_UpdatePhase3)
                {
                    Config.updateMode = Config.UpdateMode.Phase3;
                    Application.Run(new UpdateForm());
                }
                else if (args[0] == Config.Command_GetExeHash)
                {
                    //Copy app to temp
                    File.Copy(Application.ExecutablePath, Config.UpdateChecker_TempFilePath, true);

                    //Read exe
                    byte[] buffer = File.ReadAllBytes(Config.UpdateChecker_TempFilePath);

                    //Hash exe
                    string hash = CryptographicOperation.ComputeHash(buffer, SHA256.Create());

                    //Print the hash and copy to clipboard
                    Console.WriteLine(hash);
                    Clipboard.SetText(hash);

                    //Delete temp
                    File.Delete(Config.UpdateChecker_TempFilePath);
                    Console.ReadKey();
                }
                else if (args[0] == Config.Command_DebugMode)
                {
                    Debugger.Enabled = true;
                    Application.Run(new UpdateForm());
                }
                else
                {
                    Console.WriteLine("Invalid dev/internal command.");
                    return;
                }
            }
            else
            {
                Application.Run(new UpdateForm());
            }
        }
    }
}