using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class ByteEnumerableUtilsTests
    {
        [Fact]
        void Encode_encodes_bytes_to_string_with_default_format()
        {
            var bytes = new byte[] { 65, 97, 48 };
            var expectedResult = "Aa0";

            var result = bytes.Encode();

            result.Should().Be(expectedResult);
        }

        [Fact]
        void Encode_encodes_bytes_to_string_with_specified_format()
        {
            var bytes = new byte[] { 65, 97, 48 };
            var expectedResult = "Aa0";

            var result = bytes.Encode(Encoding.UTF8);

            result.Should().Be(expectedResult);
        }

        [Fact]
        void ToHexString_encodes_bytes_to_string_with_hex_format()
        {
            var bytes = new byte[] { 97, 48, 32 };
            var expectedHexString = "613020";

            var result = bytes.ToHexString();

            result.Should().Be(expectedHexString);
        }

        [Fact]
        void GetMD5Hash_computes_MD5_hash_value_of_elements()
        {
            var bytes = new byte[] { 1, 200, 30, 56, 21 };
            var md5hash = new byte[16] { 19, 162, 252, 239, 204, 59, 152, 181, 91, 6, 147, 178, 95, 52, 29, 48 };

            var result = bytes.GetMD5Hash();

            result.Should().BeEquivalentTo(md5hash);
        }

        [Fact]
        void GetSHA1Hash_computes_SHA1_hash_value_of_elements()
        {
            var bytes = new byte[] { 1, 200, 30, 56, 21 };
            var sha1hash = new byte[20] { 240, 167, 138, 94, 47, 25, 158, 241, 182, 134, 70, 119, 103, 243, 221, 232, 110, 65, 62, 63 };

            var result = bytes.GetSHA1Hash();

            result.Should().BeEquivalentTo(sha1hash);
        }

        [Fact]
        void ToStream_converts_bytes_to_a_memory_stream()
        {
            var bytes = new byte[] { 2, 56, 120, 111, 45 };
            var expectedStream = new MemoryStream(bytes);

            var result = bytes.ToStream();

            result.ToArray().Should().BeEquivalentTo(expectedStream.ToArray());
        }

        [Fact]
        void ToStream_converts_bytes_to_a_writable_memory_stream()
        {
            var bytes = new byte[] { 2, 56, 120, 111, 45 };
            var expectedStream = new MemoryStream(bytes);

            var result = bytes.ToStream(writable: true);

            result.ToArray().Should().BeEquivalentTo(expectedStream.ToArray());
            result.CanWrite.Should().BeTrue();
        }
    }
}
