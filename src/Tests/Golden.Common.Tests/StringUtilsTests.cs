using System.Text;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class StringUtilsTests
    {
        [Fact]
        void SplitLines_splits_string_by_LF_newline_separator_character()
        {
            var str = "a\nb";
            var expectedLines = new[] { "a", "b" };

            var lines = str.SplitLines();

            lines.Should().BeEquivalentTo(expectedLines);
        }

        [Fact]
        void SplitLines_splits_string_by_CR_newline_separator_character()
        {
            var str = "a\rb";
            var expectedLines = new[] { "a", "b" };

            var lines = str.SplitLines();

            lines.Should().BeEquivalentTo(expectedLines);
        }

        [Fact]
        void SplitLines_splits_string_by_CRLF_newline_separator_character()
        {
            var str = "a\r\nb";
            var expectedLines = new[] { "a", "b" };

            var lines = str.SplitLines();

            lines.Should().BeEquivalentTo(expectedLines);
        }

        [Fact]
        void SplitLines_splits_string_by_LFCR_newline_separator_character()
        {
            var str = "a\n\rb";
            var expectedLines = new[] { "a", "b" };

            var lines = str.SplitLines();

            lines.Should().BeEquivalentTo(expectedLines);
        }

        [Fact]
        void SplitLines_splits_string_and_removes_empty_entries()
        {
            var str = "a\n\nb";
            var expectedLines = new[] { "a", "b" };

            var lines = str.SplitLines(removeEmptyEntries: true);

            lines.Should().BeEquivalentTo(expectedLines);
        }

        [Fact]
        void SplitLines_splits_string_and_preserve_empty_entries()
        {
            var str = "a\n\nb";
            var expectedLines = new[] { "a", "", "b" };

            var lines = str.SplitLines(removeEmptyEntries: false);

            lines.Should().BeEquivalentTo(expectedLines);
        }

        [Fact]
        void Left_extracts_number_of_characters_from_string_starting_from_left()
        {
            var str = "abc";
            var length = 2;
            var expected = "ab";

            var result = str.Left(2);

            result.Should().Be(expected);
        }

        [Fact]
        void Right_extracts_number_of_characters_from_string_starting_from_right()
        {
            var str = "abc";
            var length = 2;
            var expected = "bc";

            var result = str.Right(2);

            result.Should().Be(expected);
        }

        [Fact]
        void Repeat_repeats_string_by_specified_number_of_time()
        {
            var str = "ab";

            var result = str.Repeat(2);

            result.Should().Be("abab");
        }

        [Fact]
        void Replace_replaces_characters_with_specified_start_and_length_with_new_value()
        {
            var str = "abcd";
            var (start, count) = (1, 2);
            var newValue = "_";
            var expectedResult = "a_d";

            var result = str.Replace(start, count, newValue);

            result.Should().Be(expectedResult);
        }

        [Fact]
        void Reverse_reverses_a_string()
        {
            var str = "abcd";
            var expectedResult = "dcba";

            var result = str.Reverse();

            result.Should().Be(expectedResult);
        }

        [Fact]
        void Concat_concat_passed_string_with_specified_strings()
        {
            var str = "ab";

            var result = str.Concat("c", "d");

            result.Should().Be("abcd");
        }

        [Fact]
        void IsMatch_returns_true_when_string_is_matched_with_specified_pattern()
        {
            var str = "aB1";
            var pattern = "[a-z][A-Z]\\d";

            var result = str.IsMatch(pattern);

            result.Should().BeTrue();
        }

        [Fact]
        void IsMatch_checks_string_is_matched_with_specified_pattern_casesensitive()
        {
            var str = "A";
            var pattern = "[a-z]";

            var result = str.IsMatch(pattern, ignoreCase: false);

            result.Should().BeFalse();
        }

        [Fact]
        void IsMatch_checks_string_is_matched_with_specified_pattern_caseinsensitive()
        {
            var str = "A";
            var pattern = "[a-z]";

            var result = str.IsMatch(pattern, ignoreCase: true);

            result.Should().BeTrue();
        }

        [Fact]
        void EncodeBase64_encodes_string_to_Base64_string()
        {
            var str = "ABCD";
            var expected = "QUJDRA==";

            var result = str.EncodeBase64();

            result.Should().Be(expected);
        }

        [Fact]
        void EncodeBase64_encodes_string_to_Base64_string_with_specified_encoding()
        {
            var str = "ABCD";
            var expected = "QQAAAEIAAABDAAAARAAAAA==";

            var result = str.EncodeBase64(Encoding.UTF32);

            result.Should().Be(expected);
        }

        [Fact]
        void DecodeBase64_decodes_string_from_Base64_string()
        {
            var str = "QUJDRA==";
            var expected = "ABCD";

            var result = str.DecodeBase64();

            result.Should().Be(expected);
        }

        [Fact]
        void DecodeBase64_decodes_string_from_Base64_string_with_specified_encoding()
        {
            var str = "QQAAAEIAAABDAAAARAAAAA==";
            var expected = "ABCD";

            var result = str.DecodeBase64(Encoding.UTF32);

            result.Should().Be(expected);
        }

        [Fact]
        void Format_formats_string_with_specified_named_parameters()
        {
            var str = "{Code}:{Name}";
            var parametersObject = new
            {
                Code = 10,
                Name = "Ali"
            };
            var expected = "10:Ali";

            var result = str.Format(parametersObject);

            result.Should().Be(expected);
        }

        [Fact]
        void GetBytes_returns_bytes_of_string()
        {
            var str = "AB1";
            var expected = new byte[] { 65, 66, 49 };

            var result = str.GetBytes();

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        void GetBytes_returns_bytes_of_string_with_specified_encoding()
        {
            var str = "AB1";
            var expected = new byte[] { 65, 0, 0, 0, 66, 0, 0, 0, 49, 0, 0, 0 };

            var result = str.GetBytes(Encoding.UTF32);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        void EqualsOrdinal_returns_true_when_two_passed_strings_are_equal()
        {
            var value = "ABC";
            var anotherValue = "ABC";
            var result = value.EqualsOrdinal(anotherValue);

            result.Should().BeTrue();
        }

        [Fact]
        void EqualsOrdinal_returns_true_when_two_passed_strings_are_equal_ignore_case()
        {
            var value = "ABC";
            var anotherValue = "abc";
            var result = value.EqualsOrdinal(anotherValue, ignoreCase: true);

            result.Should().BeTrue();
        }

        [Fact]
        void EqualsOrdinal_returns_false_when_two_passed_strings_are_not_equal()
        {
            var value = "ABC";
            var anotherValue = "BDC";
            var result = value.EqualsOrdinal(anotherValue);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("a", "c", -2)]
        [InlineData("c", "a", 2)]
        [InlineData("a", "a", 0)]
        [InlineData("A", "a", -32)]
        void EqualsOrdinal_compares_two_passed_strings(
            string value,
            string anotherValue,
            int expectedCompareResult)
        {
            var result = value.CompareOrdinal(anotherValue);

            result.Should().Be(expectedCompareResult);
        }

        [Fact]
        void EqualsOrdinal_compares_two_passed_strings_ignore_case()
        {
            var value = "A";
            var anotherValue = "a";

            var result = value.CompareOrdinal(anotherValue, ignoreCase: true);

            result.Should().Be(0);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", " ")]
        [InlineData("a", "a")]
        void EmptyAsNull_returns_NULL_when_empty_string_is_passed(string value, string expectedResult)
        {
            var result = value.EmptyAsNull();

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData("\t", null)]
        [InlineData("\n", null)]
        [InlineData(" a ", "a")]
        [InlineData("a", "a")]
        void TrimToNull_returns_NULL_when_blank_string_is_passed(string value, string expectedResult)
        {
            var result = value.TrimToNull();

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", false)]
        [InlineData("a", false)]
        void IsEmpty_returns_true_when_empty_string_is_passed(string value, bool expectedResult)
        {
            var result = value.IsEmpty();

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", true)]
        [InlineData("a", true)]
        void IsNotEmpty_returns_false_when_empty_string_is_passed(string value, bool expectedResult)
        {
            var result = value.IsNotEmpty();

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData("a", false)]
        void IsBlank_returns_true_when_blank_string_is_passed(string value, bool expectedResult)
        {
            var result = value.IsBlank();

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("a", true)]
        void IsNotBlank_returns_false_when_blank_string_is_passed(string value, bool expectedResult)
        {
            var result = value.IsNotBlank();

            result.Should().Be(expectedResult);
        }
    }
}
