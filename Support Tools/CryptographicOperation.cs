
namespace Main.Support_Tools
{
    public static class CryptographicOperation
    {
        //File processing with custom settings
        public static void ProcessFiles(List<string> files, string password, int maxParallelTasks, Config.CryptographyMode mode, IProgress<(string file, bool success)> generalProgress, IProgress<string> fileProgress)
        {
            ParallelOptions options = new() { MaxDegreeOfParallelism = maxParallelTasks };

            Parallel.ForEach(files, options, file =>
            {
                bool success = true;
                fileProgress.Report(file);

                try
                {
                    string tempFile = file + ".tmp";

                    byte[] data = File.ReadAllBytes(file);
                    byte[] processed = mode == Config.CryptographyMode.Encrypt
                        ? Tool.EncryptData(data, password)
                        : Tool.DecryptData(data, password);
                    File.WriteAllBytes(file, processed);
                }
                catch
                {
                    success = false;
                }

                // Report progress to UI safely
                generalProgress.Report((file, success));
                
            });
        }

        //All parallel threads called for actions
        public static void ProcessFiles(List<string> files, string password, Config.CryptographyMode mode, IProgress<(string file, bool success)> progress, IProgress<string> fileProgress)
        {

            Parallel.ForEach(files, file =>
            {
                bool success = true;
                fileProgress.Report(file);

                try
                {
                    byte[] data = File.ReadAllBytes(file);
                    byte[] processed = mode == Config.CryptographyMode.Encrypt
                        ? Tool.EncryptData(data, password)
                        : Tool.DecryptData(data, password);
                    File.WriteAllBytes(file, processed);
                }
                catch
                {
                    success = false;
                }

                // Report progress to UI safely
                progress.Report((file, success));
            });
        }
    }
}
