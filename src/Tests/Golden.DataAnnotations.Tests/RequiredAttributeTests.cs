using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Golden.DataAnnotations.Tests
{
    public class RequiredAttributeTests
    {
        private RequiredAttribute validator;

        public RequiredAttributeTests()
        {
            validator = new RequiredAttribute();
        }

        [Fact]
        void IsValid_returns_false_when_null_value_passed()
        {
            var result = validator.IsValid(value: null);

            result.Should().BeFalse();
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
        void IsValid_returns_false_when_not_defined_enum_value_passed()
        {
            var value = (Gender)(0);

            var result = validator.IsValid(value);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        void IsValid_returns_false_when_empty_string_passed(string value)
        {
            validator.AllowEmptyStrings = false;

            var result = validator.IsValid(value);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        void IsValid_returns_true_when_empty_string_passed_with_AllowEmptyStrings_flag(string value)
        {
            validator.AllowEmptyStrings = true;

            var result = validator.IsValid(value);

            result.Should().BeTrue();
        }

        [Fact]
        void IsValid_returns_true_when_default_value_passed()
        {
            validator.AllowDefaultValues = true;

            var result = validator.IsValid(default(Guid));

            result.Should().BeTrue();
        }

        public static IEnumerable<object[]> DefaultValues = new List<object[]>
        {
            new object[] { default(DateTime) },
            new object[] { default(DateTimeOffset) },
            new object[] { default(TimeSpan) },
            new object[] { default(Guid) },
            new object[] { default(Char) },
            new object[] { default(Int32) }
        };
        [Theory]
        [MemberData(nameof(DefaultValues))]
        void IsValid_returns_false_when_default_value_passed_with_AllowDefaultValues_flag(object value)
        {
            validator.AllowDefaultValues = false;

            var result = validator.IsValid(value);

            result.Should().BeFalse();
        }
    }
}
