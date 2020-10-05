using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Golden.DataAnnotations.Tests
{
    public class NoDefaultAttributeTests
    {
        private NoDefaultAttribute validator;

        public NoDefaultAttributeTests()
        {
            validator = new NoDefaultAttribute();
        }

        [Fact]
        void IsValid_returns_true_when_null_value_passed()
        {
            var result = validator.IsValid(value: null);

            result.Should().BeTrue();
        }

        [Fact]
        void IsValid_returns_true_when_a_value_passed()
        {
            object value = 1;

            var result = validator.IsValid(value);

            result.Should().BeTrue();
        }

        enum Gender { Male = 1, Female = 2 }
        [Fact]
        void IsValid_returns_false_when_default_enum_value_passed()
        {
            var value = default(Gender);

            var result = validator.IsValid(value);

            result.Should().BeFalse();
        }

        public static IEnumerable<object[]> DefaultValues = new List<object[]> {
            new object[] { default(DateTime) },
            new object[] { default(DateTimeOffset) },
            new object[] { default(TimeSpan) },
            new object[] { default(Guid) },
            new object[] { default(Char) },
            new object[] { default(Int32) }
        };
        [Theory]
        [MemberData(nameof(DefaultValues))]
        void IsValid_returns_false_when_default_value_passed(object value)
        {
            var result = validator.IsValid(value);

            result.Should().BeFalse();
        }
    }
}
