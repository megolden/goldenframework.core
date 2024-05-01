using FluentAssertions;
using Xunit;

namespace Golden.Common.Persian.Tests
{
    public class NumberPersianUtilsTests
    {
        [Theory]
        [InlineData(0, "صفر")]
        [InlineData(1, "یک")]
        [InlineData(2, "دو")]
        [InlineData(3, "سه")]
        [InlineData(4, "چهار")]
        [InlineData(5, "پنج")]
        [InlineData(6, "شش")]
        [InlineData(7, "هفت")]
        [InlineData(8, "هشت")]
        [InlineData(9, "نه")]
        void NumberToLetter_returns_persian_number_name_when_one_digit_number_passed(
            long number,
            string expectedName)
        {
            var name = NumberPersianUtils.NumberToLetter(number);

            name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(10, "ده")]
        [InlineData(11, "یازده")]
        [InlineData(12, "دوازده")]
        [InlineData(13, "سیزده")]
        [InlineData(14, "چهارده")]
        [InlineData(15, "پانزده")]
        [InlineData(16, "شانزده")]
        [InlineData(17, "هفده")]
        [InlineData(18, "هجده")]
        [InlineData(19, "نوزده")]
        void NumberToLetter_returns_persian_number_name_when_two_digit_small_number_passed(
            long number,
            string expectedName)
        {
            var name = NumberPersianUtils.NumberToLetter(number);

            name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(20, "بیست")]
        [InlineData(30, "سی")]
        [InlineData(40, "چهل")]
        [InlineData(50, "پنجاه")]
        [InlineData(60, "شصت")]
        [InlineData(70, "هفتاد")]
        [InlineData(80, "هشتاد")]
        [InlineData(90, "نود")]
        [InlineData(45, "چهل و پنج")]
        void NumberToLetter_returns_persian_number_name_when_two_digit_number_passed(
            long number,
            string expectedName)
        {
            var name = NumberPersianUtils.NumberToLetter(number);

            name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(100, "صد")]
        [InlineData(200, "دویست")]
        [InlineData(300, "سیصد")]
        [InlineData(400, "چهارصد")]
        [InlineData(500, "پانصد")]
        [InlineData(600, "ششصد")]
        [InlineData(700, "هفتصد")]
        [InlineData(800, "هشتصد")]
        [InlineData(900, "نهصد")]
        [InlineData(220, "دویست و بیست")]
        [InlineData(301, "سیصد و یک")]
        [InlineData(317, "سیصد و هفده")]
        [InlineData(450, "چهارصد و پنجاه")]
        [InlineData(228, "دویست و بیست و هشت")]
        void NumberToLetter_returns_persian_number_name_when_three_digit_number_passed(
            long number,
            string expectedName)
        {
            var name = NumberPersianUtils.NumberToLetter(number);

            name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(1000, "هزار")]
        [InlineData(1100, "هزار و صد")]
        [InlineData(2000, "دو هزار")]
        [InlineData(2300, "دو هزار و سیصد")]
        [InlineData(4212, "چهار هزار و دویست و دوازده")]
        [InlineData(6500, "شش هزار و پانصد")]
        [InlineData(6001, "شش هزار و یک")]
        [InlineData(2017, "دو هزار و هفده")]
        [InlineData(2901, "دو هزار و نهصد و یک")]
        void NumberToLetter_returns_persian_number_name_when_four_digit_number_passed(
            long number,
            string expectedName)
        {
            var name = NumberPersianUtils.NumberToLetter(number);

            name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(1001000, "یک میلیون و هزار")]
        [InlineData(1002000, "یک میلیون و دو هزار")]
        [InlineData(1000100, "یک میلیون و صد")]
        [InlineData(1001100, "یک میلیون و هزار و صد")]
        [InlineData(1000000, "یک میلیون")]
        [InlineData(2000000, "دو میلیون")]
        [InlineData(2000000000, "دو میلیارد")]
        [InlineData(2000000100, "دو میلیارد و صد")]
        [InlineData(2000000001, "دو میلیارد و یک")]
        [InlineData(2000000019, "دو میلیارد و نوزده")]
        [InlineData(10000000000, "ده میلیارد")]
        [InlineData(1000000000000, "یک تریلیون")]
        [InlineData(1000000000000000, "یک کوآدریلیون")]
        [InlineData(1000000000000000000, "یک کوینتیلیون")]
        void NumberToLetter_returns_persian_number_name_when_more_than_four_digit_number_passed(
            long number,
            string expectedName)
        {
            var name = NumberPersianUtils.NumberToLetter(number);

            name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(-1, "منفی یک")]
        [InlineData(-100, "منفی صد")]
        [InlineData(-1000, "منفی هزار")]
        void NumberToLetter_returns_persian_number_name_when_negate_number_passed(
            long number,
            string expectedName)
        {
            var name = NumberPersianUtils.NumberToLetter(number);

            name.Should().Be(expectedName);
        }
    }
}
