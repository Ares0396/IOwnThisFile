namespace Main.Support_Tools
{
    public class LockStream
    {
        public static FileStream Initialize(string filePath)
        {
            FileStream lockStream;
            try
            {
                // Create a FileStream with exclusive access to the file
                lockStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                return lockStream;
            }
            catch (IOException ex)
            {
                // Handle the case where the file is already locked or inaccessible
                Console.WriteLine($"Error initializing lock stream: {ex.Message}");
                return null;
            }
        }
        public static void Close(FileStream lockStream)
        {
            lockStream?.Dispose();
        }
    }
}
