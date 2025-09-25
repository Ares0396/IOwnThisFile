namespace Main.Support_Tools
{
    public class ByteUnit
    {
        public static string ConvertRawBytes(long size)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB" };
            double len = size;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Format with 3 decimal places
            return $"{len:0.###} {sizes[order]}";
        }
    }
}
