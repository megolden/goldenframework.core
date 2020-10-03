using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class StreamUtilsTests
    {
        [Fact]
        void ToBytes_converts_a_stream_to_byte_array()
        {
            var expectedBytes = new byte[] { 1, 2, 3, 4, 5 };
            using Stream stream = new MemoryStream(expectedBytes);

            var bytes = stream.ToBytes();

            bytes.Should().BeEquivalentTo(expectedBytes, _ => _.WithStrictOrdering());
        }

        [Fact]
        void ToBytes_converts_a_stream_with_NonZero_position_to_byte_array()
        {
            var expectedBytes = new byte[] { 1, 2, 3, 4, 5 };
            using Stream stream = new MemoryStream(expectedBytes);
            stream.Seek(offset: 3, SeekOrigin.Begin);

            var bytes = stream.ToBytes();

            bytes.Should().BeEquivalentTo(expectedBytes, _ => _.WithStrictOrdering());
        }

        [Fact]
        void ToBytes_converts_a_stream_to_byte_array_without_position_change()
        {
            var expectedBytes = new byte[] { 1, 2, 3, 4, 5 };
            long position = 3;
            using Stream stream = new MemoryStream(expectedBytes);
            stream.Seek(position, SeekOrigin.Begin);

            var bytes = stream.ToBytes();

            bytes.Should().BeEquivalentTo(expectedBytes, _ => _.WithStrictOrdering());
            stream.Position.Should().Be(position);
        }

        [Fact]
        void ReadAsString_reads_a_stream_as_string()
        {
            var stream = new MemoryStream(new byte[] { 65, 66, 48 });
            var expectedString = "AB0";

            var result = stream.ReadAsString();

            result.Should().Be(expectedString);
        }

        [Fact]
        void ReadAsString_reads_a_stream_as_string_with_ASCII_encoding()
        {
            var stream = new MemoryStream(new byte[] { 65, 66, 48 });
            var expectedString = "AB0";

            var result = stream.ReadAsString(Encoding.ASCII);

            result.Should().Be(expectedString);
        }
    }
}
