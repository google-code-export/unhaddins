using System;
using System.Security.Cryptography;

namespace uNhAddIns.Example.AspNetMVCConversationUsage.Utils
{
    public class NameHelper {
        public static string GetRandomNameWithLength(int count) {
            var random = new RNGCryptoServiceProvider();
            var buffer = new byte[count];
            random.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}