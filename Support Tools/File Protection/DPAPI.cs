using System.Security.Cryptography;

namespace AIO.Support_Tools.File_Protection
{
    public class DPAPI
    {
        public static byte[] Encrypt(byte[] data, byte[] entropy)
        {
            return ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
        }
        public static byte[] Decrypt(byte[] data, byte[] entropy)
        {
            return ProtectedData.Unprotect(data, entropy, DataProtectionScope.CurrentUser);
        }
    }
}
