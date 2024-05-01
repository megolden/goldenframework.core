using System;

namespace Golden.Common.Persian
{
    public static class PersianDateTimeExtensions
    {
        public static PersianDateTime ToPersian(this DateTime date)
        {
            return PersianDateTime.FromDateTime(date);
        }

        public static PersianDateTime FirstDayOfYear(this PersianDateTime date)
        {
            return new PersianDateTime(date.Year, 1, 1);
        }

        public static PersianDateTime LastDayOfYear(this PersianDateTime date)
        {
            return new PersianDateTime(
                date.Year, month: 12, day: PersianDateTime.DaysInMonth(date.Year, 12),
                hour: 23, minute: 59, second: 59, millisecond: 999);
        }
    }
}
