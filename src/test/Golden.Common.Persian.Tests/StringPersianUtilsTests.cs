using FluentAssertions;
using Xunit;

namespace Golden.Common.Persian.Tests
{
    public class StringPersianUtilsTests
    {
        [Fact]
        void ToPersianDigits_replaces_latin_digit_chars_with_persian_native_digits()
        {
            var result = "Digits: 0123456789".ToPersianDigits();

            result.Should().Be("Digits: ۰۱۲۳۴۵۶۷۸۹");
        }

        [Fact]
        void ToLatinDigits_replaces_persian_native_digit_chars_with_latin_digits()
        {
            var result = "Digits: ۰۱۲۳۴۵۶۷۸۹".ToLatinDigits();

            result.Should().Be("Digits: 0123456789");
        }

        [Fact]
        void ToLatinDigits_replaces_arabic_native_digit_chars_with_latin_digits()
        {
            var result = "Digits: ٠١٢٣٤٥٦٧٨٩".ToLatinDigits(alsoArabicDigits: true);

            result.Should().Be("Digits: 0123456789");
        }

        [Fact]
        void ToPersianKafYeh_replaces_arabic_letters_kaf_and_yeh_letters_with_persian_kaf_yeh()
        {
            var result = "كي بود".ToPersianKafYeh();

            result.Should().Be("کی بود");
        }
    }
}
