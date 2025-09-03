using Main.Support_Tools;
using System.Security.Cryptography;

namespace Main
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
                    string hash = Tool.HashContent(buffer, SHA256.Create());

                    //Print the hash and copy to clipboard
                    Console.WriteLine(hash);
                    Clipboard.SetText(hash);

                    //Delete temp
                    File.Delete(Config.UpdateChecker_TempFilePath);
                    Console.ReadKey();
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