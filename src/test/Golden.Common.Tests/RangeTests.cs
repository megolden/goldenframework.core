using System;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class RangeTests
    {
        [Fact]
        void Constructor_constructs_a_Range_instance_properly()
        {
            var range = new Range<int>(min: 1, max: 5);

            range.Min.Should().Be(1);
            range.Max.Should().Be(5);
        }

        [Fact]
        void Constructor_fails_when_invalid_min_value_passed()
        {
            var maxValue = 5;
            var minValueGreaterThanMaxValue = 6;
            Action create = () => new Range<int>(minValueGreaterThanMaxValue, maxValue);

            create.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        void Constructor_fails_when_invalid_max_value_passed()
        {
            var minValue = 6;
            var maxValueLessThanMinValue = 5;
            Action create = () => new Range<int>(minValue, maxValueLessThanMinValue);

            create.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        void Includes_returns_true_when_a_value_in_range_passed()
        {
            var range = new Range<int>(2, 5);
            var value = 4;

            var result = range.Includes(value);

            result.Should().BeTrue();
        }

        [Fact]
        void Includes_returns_false_when_a_value_out_of_range_passed()
        {
            var range = new Range<int>(2, 5);
            var value = 6;

            var result = range.Includes(value);

            result.Should().BeFalse();
        }
    }
}
