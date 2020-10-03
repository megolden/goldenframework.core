using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class BooleanUtilsTests
    {
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        void And_applies_logical_AND_operator_on_arguments(bool arg1, bool arg2, bool expected)
        {
            var actual = arg1.And(arg2);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(false, false, false)]
        void Or_applies_logical_OR_operator_on_arguments(bool arg1, bool arg2, bool expected)
        {
            var actual = arg1.Or(arg2);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        void AndNot_applies_logical_AND_NOT_operator_on_arguments(bool arg1, bool arg2, bool expected)
        {
            var actual = arg1.AndNot(arg2);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        void OrNot_applies_logical_OR_NOT_operator_on_arguments(bool arg1, bool arg2, bool expected)
        {
            var actual = arg1.OrNot(arg2);

            actual.Should().Be(expected);
        }
    }
}
