using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class EqualityComparerTests
    {
        [Theory]
        [InlineData(2, 4)]
        [InlineData(100, 12)]
        void Equals_returns_true_when_equal_numbers_passed(int n1, int n2)
        {
            var evenEqualityComparer = new EqualityComparer<int>(
                (x, y) => x % 2 == 0 && y % 2 == 0);

            var equalityResult = evenEqualityComparer.Equals(n1, n2);

            equalityResult.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(100, 13)]
        [InlineData(1, 13)]
        void Equals_returns_false_when_no_equal_numbers_passed(int n1, int n2)
        {
            var evenEqualityComparer = new EqualityComparer<int>(
                (x, y) => x % 2 == 0 && y % 2 == 0);

            var equalityResult = evenEqualityComparer.Equals(n1, n2);

            equalityResult.Should().BeFalse();
        }

        [Theory]
        [InlineData(100L)]
        [InlineData(12435436576L)]
        void GetHashCode_returns_default_argument_hashcode(long n)
        {
            var someEqualityComparer = new EqualityComparer<long>((_, __) => true);
            var expectedHashCode = n.GetHashCode();

            var hashcodeResult = someEqualityComparer.GetHashCode(n);

            hashcodeResult.Should().Be(expectedHashCode);
        }

        [Theory]
        [InlineData(100L, 90)]
        [InlineData(12435436576L, 12345)]
        void GetHashCode_returns_hashcode_using_specified_function(long n, int expectedHashCode)
        {
            var someEqualityComparer = new EqualityComparer<long>((_, __) => true,
                num => expectedHashCode);

            var hashcodeResult = someEqualityComparer.GetHashCode(n);

            hashcodeResult.Should().Be(expectedHashCode);
        }

        [Fact]
        void By_creates_equality_comparer_by_specified_property_equality_and_hashcode()
        {
            var idEqualityComparer = EqualityComparer<ECBook>.By(_ => _.Code);
            var book1 = new ECBook { Code = 10 };
            var book2 = new ECBook { Code = 10 };

            var equalityResult = idEqualityComparer.Equals(book1, book2);
            var book1Hashcode = idEqualityComparer.GetHashCode(book1);
            var book2Hashcode = idEqualityComparer.GetHashCode(book2);

            equalityResult.Should().BeTrue();
            book1Hashcode.Should().Be(book1.Code.GetHashCode());
            book2Hashcode.Should().Be(book2.Code.GetHashCode());
        }
    }

    class ECBook
    {
        public int Code { get; set; }
    }
}
