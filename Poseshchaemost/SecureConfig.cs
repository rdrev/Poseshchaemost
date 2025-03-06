using System;
using System.Security.Cryptography;
using System.Text;

namespace Poseshchaemost
{
    public static class SecureConfig
    {
        public static string Encrypt(string plainText)
        {
            byte[] encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(plainText),
                null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] decrypted = ProtectedData.Unprotect(Convert.FromBase64String(encryptedText),
                null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
