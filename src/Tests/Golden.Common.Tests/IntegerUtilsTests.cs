using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class IntegerUtilsTests
    {
        [Theory]
        [InlineData(10, 3)]
        [InlineData(9, 3)]
        void DivideRemainder_returns_remainder_of_division_of_int_arguments(int value, int divisor)
        {
            var expectedRemainder = value % divisor;

            var result = value.DivideRemainder(divisor);

            result.Should().Be(expectedRemainder);
        }

        [Theory]
        [InlineData(10L, 3L)]
        [InlineData(9L, 3L)]
        void DivideRemainder_returns_remainder_of_division_of_long_arguments(long value, long divisor)
        {
            var expectedRemainder = value % divisor;

            var result = value.DivideRemainder(divisor);

            result.Should().Be(expectedRemainder);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(1234)]
        void IsEven_returns_true_when_int_even_number_passed(int value)
        {
            var result = value.IsEven();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(11)]
        [InlineData(12345)]
        void IsEven_returns_false_when_int_odd_number_passed(int value)
        {
            var result = value.IsEven();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(103266L)]
        [InlineData(943444L)]
        void IsEven_returns_true_when_long_even_number_passed(long value)
        {
            var result = value.IsEven();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(11L)]
        [InlineData(12345L)]
        void IsEven_returns_false_when_long_odd_number_passed(long value)
        {
            var result = value.IsEven();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(11)]
        [InlineData(12345)]
        void IsOdd_returns_true_when_int_odd_number_passed(int value)
        {
            var result = value.IsOdd();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(12)]
        [InlineData(1234)]
        void IsOdd_returns_false_when_int_even_number_passed(int value)
        {
            var result = value.IsOdd();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1032667L)]
        [InlineData(9434441L)]
        void IsOdd_returns_true_when_long_odd_number_passed(long value)
        {
            var result = value.IsOdd();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(2L)]
        [InlineData(114L)]
        [InlineData(123448L)]
        void IsOdd_returns_false_when_long_even_number_passed(long value)
        {
            var result = value.IsOdd();

            result.Should().BeFalse();
        }
    }
}
