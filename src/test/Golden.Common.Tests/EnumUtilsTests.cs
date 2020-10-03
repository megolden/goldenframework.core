using System;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class EnumUtilsTests
    {
        [Fact]
        void Has_returns_true_when_enum_includes_specified_item()
        {
            var letter = Letter.A | Letter.B;

            var result = letter.Has(Letter.B);

            result.Should().BeTrue();
        }

        [Fact]
        void Has_returns_false_when_enum_not_includes_specified_item()
        {
            var letter = Letter.A | Letter.B;

            var result = letter.Has(Letter.C);

            result.Should().BeFalse();
        }

        [Fact]
        void Set_append_specified_enum_values()
        {
            var letter = Letter.A;

            var result = letter.Set(Letter.B, Letter.C);

            result.HasFlag(Letter.A).Should().BeTrue();
            result.HasFlag(Letter.B).Should().BeTrue();
            result.HasFlag(Letter.C).Should().BeTrue();
        }

        [Fact]
        void Unset_removes_specified_enum_values()
        {
            var letter = Letter.A | Letter.B | Letter.C;

            var result = letter.Unset(Letter.A, Letter.B);

            result.HasFlag(Letter.A).Should().BeFalse();
            result.HasFlag(Letter.B).Should().BeFalse();
            result.HasFlag(Letter.C).Should().BeTrue();
        }

        [Fact]
        void HasAllFlags_returns_true_when_value_includes_all_enum_values()
        {
            var letter = Letter.A | Letter.B | Letter.C;

            var result = letter.HasAllFlags();

            result.Should().BeTrue();
        }

        [Fact]
        void HasAllFlags_returns_false_when_at_least_one_enum_value_not_included_in_value()
        {
            var letter = Letter.A | Letter.B;

            var result = letter.HasAllFlags();

            result.Should().BeFalse();
        }

        [Flags]
        enum Letter
        {
            A = 1,
            B = 2,
            C = 4
        }
    }
}
