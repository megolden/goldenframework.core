using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Golden.Common.Persian
{
    [Serializable]
	[TypeConverter(typeof(PersianDateTimeConverter))]
	public struct PersianDateTime :
        IFormattable,
        IComparable<PersianDateTime>,
        IEquatable<PersianDateTime>,
        IComparable,
        IConvertible,
        IEqualityComparer<PersianDateTime>
	{
        #region Constants

        private const long TICKS_PER_YEAR = 315360000000000L; // 365 Days

        private const long TICKS_PER_LEAP_YEAR = 316224000000000; // 366 Days

        private const long UTC_OFFSET_TICKS = 196036290000000000L;

        private const long MAX_TICKS = 3155378975999990000L; // 9999/12/29 23:59:59.999

        private const int MAX_YEAR = 9999; // Max supported year

        public static readonly TimeSpan DaylightDelta = TimeSpan.FromHours(1);

        private static readonly (TimeSpan Start, TimeSpan End) DaylightSavingTimeRange =
            (new TimeSpan(1, 1, 0, 0, 0), new TimeSpan(5 * 31 + 29, 22, 59, 59, 999));

        #endregion

        private static readonly PersianDateTimeParser _parser = new();
        private static readonly PersianDateTimeFormatter _formatter = new();

        #region Properties

        public static readonly PersianDateTime MinValue = new PersianDateTime(0L);

        public static readonly PersianDateTime MaxValue = new PersianDateTime(MAX_TICKS);

		public static PersianDateTime Today
        {
            get => Now.Date;
        }

        public static PersianDateTime Now
		{
            get => FromDateTime(DateTime.Now);
		}

		public PersianDateTime Date
		{
            get => new PersianDateTime(Year, Month, Day);
		}

		public string DayName { get; private set; }

		public string MonthName { get; private set; }

        public long Ticks { get; private set; }

        public int Year { get; private set; }

		public int Month { get; private set; }

		public int Day { get; private set; }

		public int Hour { get; private set; }

		public int Hour12 { get; private set; }

		public bool IsAfternoon { get; private set; }

		public int Minute { get; private set; }

		public int Second { get; private set; }

		public int Millisecond { get; private set; }

		public DayOfWeek DayOfWeek { get; private set; }

		public int DayOfYear { get; private set; }

		public TimeSpan TimeOfDay { get; private set; }

		public TimeSpan TimeOfYear { get; private set; }

		public bool IsDaylightSavingTime { get; private set; }

        #endregion

		#region Methods

        private PersianDateTime(
            long ticks,
            int year, int month, int day,
            int hour, int minute, int second, int millisecond)
        {
            Ticks = ticks;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
            DayOfWeek = GetDayOfWeek(GetWeekDay(Ticks));
            MonthName = GetMonthName(Month);
            DayName = GetDayName(DayOfWeek);
            DayOfYear = GetDayOfYear(Month, Day);
            TimeOfDay = new TimeSpan(days: 0, Hour, Minute, Second, Millisecond);
            TimeOfYear = new TimeSpan(DayOfYear - 1, Hour, Minute, Second, Millisecond);
            IsDaylightSavingTime = TimeOfYear >= DaylightSavingTimeRange.Start &&
                                   TimeOfYear <= DaylightSavingTimeRange.End;
            Hour12 = GetHour12(Hour);
            IsAfternoon = Hour >= 12;
        }

		public PersianDateTime(long ticks)
		{
			if (IsValidTicks(ticks) is false)
                throw new ArgumentException("Invalid date value.", nameof(ticks));

			GetDateParts(ticks,
                out var year, out var month, out var day,
                out var hour, out var minute, out var second, out var millisecond);

            this = new PersianDateTime(
                ticks,
                year, month, day,
                hour, minute, second, millisecond);
        }

		public PersianDateTime(
            int year, int month, int day,
            int hour, int minute, int second, int millisecond)
		{
			if (IsValid(year, month, day, hour, minute, second, millisecond) is false)
                throw new ArgumentException("Invalid date value.");

			var ticks = GetTicks(year, month, day, hour, minute, second, millisecond);

            this = new PersianDateTime(
                ticks,
                year, month, day,
                hour, minute, second, millisecond);
        }

		public PersianDateTime(
            int year, int month, int day,
            int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, 0) { }

		public PersianDateTime(int year, int month, int day)
            : this(year, month, day, 0, 0, 0, 0) { }

		private static int GetWeekDay(long ticks)
        {
            const int InitialDateWeekDayIndex = 5;
			var days = ticks / TimeSpan.TicksPerDay;
			return (int)((days + InitialDateWeekDayIndex) % 7L);
		}

		private static DayOfWeek GetDayOfWeek(int weekDay)
		{
			return (DayOfWeek)((weekDay + 6) % 7);
		}

		private static int GetDayOfYear(int month, int day)
        {
            var days = day;
            var tempMonth = month - 1;
			while (tempMonth > 0)
			{
				days = days + DaysInMonth(0, month);
                tempMonth = tempMonth - 1;
            }
			return days;
		}

		private static int GetHour12(int hour)
        {
            if (hour == 0)
                return 12;

            if (hour <= 12)
                return hour;

            return hour - 12;
		}

		public PersianDateTime AddTicks(long value)
		{
			return new PersianDateTime(Ticks + value);
		}

		public PersianDateTime Add(TimeSpan value)
		{
			return AddTicks(value.Ticks);
		}

		public PersianDateTime AddDays(long value)
		{
			return AddTicks(TimeSpan.TicksPerDay * value);
		}

		public PersianDateTime AddHours(long value)
		{
			return AddTicks(TimeSpan.TicksPerHour * value);
		}

		public PersianDateTime AddMilliseconds(long value)
		{
			return AddTicks(TimeSpan.TicksPerMillisecond * value);
		}

		public PersianDateTime AddMinutes(long value)
		{
			return AddTicks(TimeSpan.TicksPerMinute * value);
		}

		public PersianDateTime AddMonths(int value)
		{
			var year = Year;
			var month = Month;
            var day = Day;
            var subtract = value < 0;

            if (subtract)
            {
                while (value != 0)
                {
                    if (month == 1)
                    {
                        year = year - 1;
                        month = 12;
                    }
                    else
                    {
                        month = month - 1;
                    }
                    value = value + 1;
                }
            }
            else
            {
                while (value != 0)
                {
                    if (month == 12)
                    {
                        year = year + 1;
                        month = 1;
                    }
                    else
                    {
                        month = month + 1;
                    }
                    value = value - 1;
                }
            }

            var daysInMonth = DaysInMonth(year, month);
            if (day > daysInMonth) day = daysInMonth;

            return new PersianDateTime(year, month, day, Hour, Minute, Second, Millisecond);
		}

		public PersianDateTime AddSeconds(long value)
		{
			return AddTicks(TimeSpan.TicksPerSecond * value);
		}

		public PersianDateTime AddWeeks(long value)
		{
			return AddDays(7L * value);
		}

		public PersianDateTime AddYears(int value)
		{
			return AddMonths(value * 12);
		}

		public TimeSpan Subtract(PersianDateTime value)
		{
			return new TimeSpan(Ticks - value.Ticks);
		}

		public PersianDateTime Subtract(TimeSpan value)
		{
			return AddTicks(-value.Ticks);
		}

        public DateTime ToLocalTime()
        {
            return ToUniversalTime().ToLocalTime();
        }

        public DateTime ToUniversalTime()
        {
            var utcTicks = UTC_OFFSET_TICKS + Ticks;
            if (IsDaylightSavingTime) utcTicks = utcTicks - DaylightDelta.Ticks;
            return new DateTime(utcTicks, DateTimeKind.Utc);
        }

        public static string GetMonthName(int month)
        {
            return PersianCulture.NativeMonthNames[month - 1];
        }

        public static string GetDayName(DayOfWeek weekDay)
        {
            return PersianCulture.NativeDayNames[((int)weekDay + 8) % 7];
        }

        private static int GetWeekOfYear(int year, int month, int day, bool fullWeeks)
		{
			var firstDayOfYearTicks = GetTicks(year, month, 1, 0, 0, 0, 0);
			var firstDayOfYearWeekDay = GetWeekDay(firstDayOfYearTicks);
			day = GetDayOfYear(month, day);

			if (firstDayOfYearWeekDay > 0) day = day - (6 - firstDayOfYearWeekDay + 1);
			var div = Math.DivRem(day, 7, out var rem);

			if (fullWeeks is false)
			{
				if (firstDayOfYearWeekDay > 0) div = div + 1;
				if (rem > 0) div = div + 1;
			}

			return div;
		}

        public int GetWeekOfYear(bool fullWeeksOnly = false)
		{
			return GetWeekOfYear(Year, Month, Day, fullWeeksOnly);
		}

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }

		public int CompareTo(object? other)
        {
            if (other is null)
                return 1;

            if (other is PersianDateTime otherDate)
                return CompareTo(otherDate);
            else
                throw new ArgumentException("Object must be of type PersianDateTime", nameof(other));
		}

		public int CompareTo(PersianDateTime other)
		{
			return Ticks.CompareTo(other.Ticks);
		}

        public static int Compare(PersianDateTime date, PersianDateTime otherDate)
        {
            return date.CompareTo(otherDate);
        }

		public bool Equals(PersianDateTime other)
		{
			return Ticks == other.Ticks;
		}

		public override bool Equals(object? other)
		{
			return other is PersianDateTime otherDate && Equals(otherDate);
		}

		public static bool Equals(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.Equals(otherDate);
		}

        public static int DaysInMonth(int year, int month)
		{
			if (month <= 6) return 31;
			if (month <= 11) return 30;
			return IsLeapYear(year) ? 30 : 29;
		}

        public static bool IsLeapYear(int year)
        {
            // IBM ICU formula. https://github.com/unicode-org/icu/blob/aebe91cdda5293cfd0940e66d635a761ca1307d7/icu4j/main/classes/core/src/com/ibm/icu/util/PersianCalendar.java#L310

            var rem = (25 * year + 11) % 33;
            return rem < 8;

            // switch (year % 33)
            // {
            //     case 1:
            //     case 5:
            //     case 9:
            //     case 13:
            //     case 17:
            //     case 22:
            //     case 26:
            //     case 30:
            //         return true;
            //     default:
            //         return false;
            // }
        }

		public static bool IsLeapMonth(int year, int month)
		{
			return month == 12 && IsLeapYear(year);
		}

		public static bool IsLeapDay(int year, int month, int day)
		{
			return day == 30 && IsLeapMonth(year, month);
		}

        public static int DaysInYear(int year)
		{
			return IsLeapYear(year) ? 366 : 365;
		}

        private static long GetTicks(
            int year, int month, int day,
            int hour, int minute, int second, int millisecond)
        {
            long ticks = 0;
            var originalYear = year;

            year = year - 1;
            while (year > 0)
            {
                ticks = ticks + (IsLeapYear(year) ? TICKS_PER_LEAP_YEAR : TICKS_PER_YEAR);
                year = year - 1;
            }

            month = month - 1;
            while (month > 0)
            {
                ticks = ticks + (TimeSpan.TicksPerDay * DaysInMonth(originalYear, month));
                month = month - 1;
            }

            if (day > 1) ticks = ticks + ((day - 1) * TimeSpan.TicksPerDay);

            if (hour > 0) ticks = ticks + (hour * TimeSpan.TicksPerHour);

            if (minute > 0) ticks = ticks + (minute * TimeSpan.TicksPerMinute);

            if (second > 0) ticks = ticks + (second * TimeSpan.TicksPerSecond);

            if (millisecond > 0) ticks = ticks + (millisecond * TimeSpan.TicksPerMillisecond);

            return ticks;
        }

        private static void GetDateParts(
            long ticks,
            out int year, out int month, out int day,
            out int hour, out int minute, out int second, out int millisecond)
        {
            year = 1;
            month = 1;
            day = 1;
            hour = 0;
            minute = 0;
            second = 0;
            millisecond = 0;

            if (ticks == 0L)
                return;

            // year
            var tempTicks = IsLeapYear(year) ? TICKS_PER_LEAP_YEAR : TICKS_PER_YEAR;
            while (ticks >= tempTicks)
            {
                ticks = ticks - tempTicks;
                year = year + 1;
                tempTicks = IsLeapYear(year) ? TICKS_PER_LEAP_YEAR : TICKS_PER_YEAR;
            }
            if (ticks == 0L) return;

            // month
            var tempDaysInMonth = DaysInMonth(year, month) * TimeSpan.TicksPerDay;
            while (ticks >= tempDaysInMonth)
            {
                ticks = ticks - tempDaysInMonth;
                month = month + 1;
                tempDaysInMonth = DaysInMonth(year, month) * TimeSpan.TicksPerDay;
            }
            if (ticks == 0L) return;

            // day
            long remainTicks;
            day = day + (int)Math.DivRem(ticks, TimeSpan.TicksPerDay, out remainTicks);
            if (remainTicks == 0) return;

            // hour
            var tempHour = Math.DivRem(remainTicks, TimeSpan.TicksPerHour, out remainTicks);
            hour = hour + (int)tempHour;
            if (remainTicks == 0) return;

            // minute
            minute = minute + (int)Math.DivRem(remainTicks, TimeSpan.TicksPerMinute, out remainTicks);
            if (remainTicks == 0) return;

            // second
            second = second + (int)Math.DivRem(remainTicks, TimeSpan.TicksPerSecond, out remainTicks);
            if (remainTicks == 0) return;

            // millisecond
            millisecond = millisecond + (int)(remainTicks / TimeSpan.TicksPerMillisecond);
        }

        public static PersianDateTime FromDateTime(DateTime time)
        {
            var utcTicks = time.ToUniversalTime().Ticks - UTC_OFFSET_TICKS;

            var date = new PersianDateTime(utcTicks);

            return date.IsDaylightSavingTime ? date.Add(DaylightDelta) : date;
        }

        public static bool IsValid(int year, int month, int day)
        {
            return IsValid(year, month, day, 0, 0, 0, 0);
        }

        public static bool IsValid(
            int year, int month, int day,
            int hour, int minute, int second)
        {
            return IsValid(year, month, day, hour, minute, second, 0);
        }

        public static bool IsValid(
            int year, int month, int day,
            int hour, int minute, int second, int millisecond)
        {
            if (year < 1 || year > MAX_YEAR)
                return false;

            if (month < 1 || month > 12)
                return false;

            if (day < 1 || day > 31 || day > DaysInMonth(year, month))
                return false;

            if (hour < 0 || hour > 23 || minute < 0 || minute > 59 || second < 0 || second > 59)
                return false;

            if (millisecond < 0 || millisecond > 999)
                return false;

            return true;
        }

        private static bool IsValidTicks(long ticks)
        {
            return ticks >= 0L && ticks <= MAX_TICKS;
        }

        public string ToLongDateString()
		{
			return ToString("D");
		}

		public string ToLongTimeString()
		{
			return ToString("T");
		}

		public string ToShortDateString()
		{
			return ToString("d");
		}

		public string ToShortTimeString()
		{
			return ToString("t");
		}

        public string ToDateTimeString()
		{
			return ToString("yyyy/MM/dd HH:mm:ss");
		}

        public string ToDateString()
		{
			return ToString("yyyy/MM/dd");
		}

		public string ToTimeString()
		{
			return this.ToString("HH:mm:ss");
		}

		public override string ToString()
		{
			return FormatDateTime(this, format: default);
		}

        string IFormattable.ToString(string format, IFormatProvider _)
        {
            return ToString(format);
        }

        public string ToString(string format)
		{
			return FormatDateTime(this, format);
		}

        private static string FormatDateTime(PersianDateTime date, string? format)
        {
            return _formatter.Format(date, format);
		}

        public static PersianDateTime Parse(string input)
        {
            return _parser.Parse(input);
        }

		public static bool TryParse(string input, out PersianDateTime date)
        {
            return _parser.TryParse(input, out date);
        }

        #endregion

        #region IConvertible

		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.Object;
		}

		DateTime IConvertible.ToDateTime(IFormatProvider _)
        {
            return this.ToLocalTime();
        }

		string IConvertible.ToString(IFormatProvider provider)
		{
			return ToString();
		}

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType is null)
                throw new ArgumentNullException(nameof(conversionType));

            conversionType = Nullable.GetUnderlyingType(conversionType)
                             ?? conversionType;

            if (this.GetType() == conversionType)
                return this;

            if (conversionType == typeof(object))
                return this;

            IConvertible thisValue = this;
            return Type.GetTypeCode(conversionType) switch
            {
                TypeCode.Boolean   => thisValue.ToBoolean(provider),
                TypeCode.Byte      => thisValue.ToByte(provider),
                TypeCode.Char      => thisValue.ToChar(provider),
                TypeCode.DateTime  => thisValue.ToDateTime(provider),
                TypeCode.Decimal   => thisValue.ToDecimal(provider),
                TypeCode.Double    => thisValue.ToDouble(provider),
                TypeCode.Int16     => thisValue.ToInt16(provider),
                TypeCode.Int32     => thisValue.ToInt32(provider),
                TypeCode.Int64     => thisValue.ToInt64(provider),
                TypeCode.SByte     => thisValue.ToSByte(provider),
                TypeCode.Single    => thisValue.ToSingle(provider),
                TypeCode.String    => thisValue.ToString(provider),
                TypeCode.UInt16    => thisValue.ToUInt16(provider),
                TypeCode.UInt32    => thisValue.ToUInt32(provider),
                TypeCode.UInt64    => thisValue.ToUInt64(provider),
                _ => throw CreateInvalidCastException(conversionType)
            };
        }

        private Exception CreateInvalidCastException(Type conversionType)
        {
            return new InvalidCastException($"Can't cast {this.GetType().FullName} to {conversionType.FullName}");
        }

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Int64));
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Boolean));
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Byte));
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Char));
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Decimal));
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Double));
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Int16));
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Int32));
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(SByte));
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(Single));
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(UInt16));
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(UInt32));
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw CreateInvalidCastException(typeof(UInt64));
		}

        #endregion

		#region IEqualityComparer

		int IEqualityComparer<PersianDateTime>.GetHashCode(PersianDateTime other)
		{
			return other.GetHashCode();
		}

		bool IEqualityComparer<PersianDateTime>.Equals(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.Equals(otherDate);
		}

		#endregion

		#region Operators

		public static TimeSpan operator -(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.Subtract(otherDate);
		}

		public static PersianDateTime operator -(PersianDateTime date, TimeSpan value)
		{
			return date.Subtract(value);
		}

		public static PersianDateTime operator +(PersianDateTime date, TimeSpan value)
		{
			return date.Add(value);
		}

		public static bool operator <(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.CompareTo(otherDate) < 0;
		}

		public static bool operator <=(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.CompareTo(otherDate) <= 0;
		}

		public static bool operator >(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.CompareTo(otherDate) > 0;
		}

		public static bool operator >=(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.CompareTo(otherDate) >= 0;
		}

		public static bool operator ==(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.Equals(otherDate);
		}

		public static bool operator !=(PersianDateTime date, PersianDateTime otherDate)
		{
			return date.Equals(otherDate) is false;
		}

		#endregion
	}
}
