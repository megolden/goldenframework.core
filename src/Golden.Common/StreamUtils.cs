using System;
using System.IO;
using System.Text;

namespace Golden.Common
{
    public static class StreamUtils
    {
        public static byte[] ToBytes(this Stream stream)
        {
            if (stream is MemoryStream memoryStream)
                return memoryStream.ToArray();

            var buffer = new byte[stream.Length];

            var prevPos = stream.CanSeek ? stream.Position : -1;
            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            stream.Read(buffer, 0, buffer.Length);

            if (prevPos != -1)
                stream.Seek(prevPos, SeekOrigin.Begin);

            return buffer;
        }

        public static string ReadAsString(this Stream stream)
            => stream.ReadAsString(Encoding.UTF8);
        public static string ReadAsString(this Stream stream, Encoding encoding)
        {
            var prevPos = stream.CanSeek ? stream.Position : -1;
            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(stream, encoding);
            var buffer = reader.ReadToEnd();

            if (prevPos != -1)
                stream.Seek(prevPos, SeekOrigin.Begin);

            return buffer;
        }
    }
}
