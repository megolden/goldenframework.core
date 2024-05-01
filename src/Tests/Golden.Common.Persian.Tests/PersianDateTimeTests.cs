using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Persian.Tests
{
    public class PersianDateTimeTests
    {
        [Fact]
        void ToUniversalTime_converts_to_UTC_datetime()
        {
            var date = new PersianDateTime(1399, 12, 3, 7, 45, 0, 0);

            var utc = date.ToUniversalTime();

            utc.Kind.Should().Be(DateTimeKind.Utc);
            utc.As<object>().Should().BeEquivalentTo(new
            {
                Year = 2021,
                Month = 2,
                Day = 21,
                Hour = 4,
                Minute = 15,
                Second = 0,
                Millisecond = 0
            });
        }

        [Fact]
        void ToUniversalTime_converts_to_UTC_datetime_when_is_in_DST()
        {
            var date = new PersianDateTime(1400, 1, 5, 7, 45, 0, 0);

            var utc = date.ToUniversalTime();

            utc.Kind.Should().Be(DateTimeKind.Utc);
            utc.As<object>().Should().BeEquivalentTo(new
            {
                Year = 2021,
                Month = 3,
                Day = 25,
                Hour = 3,
                Minute = 15,
                Second = 0,
                Millisecond = 0
            });
        }

        [Fact]
        void FromDateTime_converts_from_datetime_when_UTC_datetime_passed()
        {
            var utcDate = new DateTime(2021, 2, 21, 5, 0, 0, 0, DateTimeKind.Utc);

            var result = PersianDateTime.FromDateTime(utcDate);

            result.As<object>().Should().BeEquivalentTo(new
            {
                Year = 1399,
                Month = 12,
                Day = 3,
                Hour = 8,
                Minute = 30,
                Second = 0,
                Millisecond = 0
            });
        }

        [Fact]
        void FromDateTime_converts_from_datetime_when_UTC_datetime_passed_in_DST()
        {
            var utcDate = new DateTime(2021, 4, 10, 5, 0, 0, 0, DateTimeKind.Utc);

            var result = PersianDateTime.FromDateTime(utcDate);

            result.As<object>().Should().BeEquivalentTo(new
            {
                Year = 1400,
                Month = 1,
                Day = 21,
                Hour = 9,
                Minute = 30,
                Second = 0,
                Millisecond = 0
            });
        }

        [Theory]
        [InlineData(1, 1, 1, DayOfWeek.Thursday)]
        [InlineData(1, 1, 2, DayOfWeek.Friday)]
        [InlineData(1, 1, 3, DayOfWeek.Saturday)]
        [InlineData(1, 1, 4, DayOfWeek.Sunday)]
        [InlineData(1, 1, 5, DayOfWeek.Monday)]
        [InlineData(1, 1, 6, DayOfWeek.Tuesday)]
        [InlineData(1, 1, 7, DayOfWeek.Wednesday)]
        [InlineData(1, 1, 8, DayOfWeek.Thursday)]
        void WeekDay_works_properly(int year, int month, int day, DayOfWeek dayOfWeek)
        {
            var actualDayOfWeek = new PersianDateTime(year, month, day).DayOfWeek;

            actualDayOfWeek.Should().Be(dayOfWeek);
        }
    }
}
