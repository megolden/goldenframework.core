using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Golden.Common
{
    public static class ByteEnumerableUtils
    {
        public static string Encode(this IEnumerable<byte> bytes)
            => bytes.Encode(Encoding.UTF8);
        public static string Encode(this IEnumerable<byte> bytes, Encoding encoding)
        {
            return encoding.GetString(bytes.ToArray());
        }

        public static string ToHexString(this IEnumerable<byte> bytes)
        {
            return bytes.Select(_ => _.ToString("x2")).Join();
        }

        public static byte[] GetMD5Hash(this IEnumerable<byte> bytes)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(bytes.ToArray());
        }

        public static byte[] GetSHA1Hash(this IEnumerable<byte> bytes)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(bytes.ToArray());
        }

        public static MemoryStream ToStream(this IEnumerable<byte> bytes)
        {
            return bytes.ToStream(writable: false);
        }

        public static MemoryStream ToStream(this IEnumerable<byte> bytes, bool writable)
        {
            return new MemoryStream(bytes.ToArray(), writable);
        }
    }
}
