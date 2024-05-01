using System;
using System.Globalization;
using System.Linq;

namespace Golden.Common.Persian
{
    public class PersianCulture
    {
        public static readonly string[] NativeDayNames =
        {
            "شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه"
        };

        public static readonly string[] NativeMonthNames =
        {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };

        public static readonly string[] NativeDigits =
        {
            "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"
        };

        public static readonly CultureInfo Culture;

        static PersianCulture()
        {
            var culture = CultureInfo.CreateSpecificCulture("fa-IR");
            culture.NumberFormat = GetCultureNumberFormat(NativeDigits);
            culture.DateTimeFormat = GetCultureDateTimeFormat(NativeDayNames, NativeMonthNames);
            Culture = CultureInfo.ReadOnly(culture);
        }

        private static DateTimeFormatInfo GetCultureDateTimeFormat(
            string[] nativeDayNames,
            string[] nativeMonthNames)
        {
            var dayNames = new[]
            {
                nativeDayNames[1], nativeDayNames[2], nativeDayNames[3], nativeDayNames[4],
                nativeDayNames[5], nativeDayNames[6], nativeDayNames[0]
            };

            var abbreviatedDayNames = dayNames.Select(_ => _.Substring(0, 1)).ToArray();

            var monthNames = nativeMonthNames.Append(String.Empty).ToArray();

            return new DateTimeFormatInfo
            {
                DayNames = dayNames,
                AbbreviatedDayNames = abbreviatedDayNames,
                ShortestDayNames = abbreviatedDayNames.ToArray(),
                MonthNames = monthNames,
                AbbreviatedMonthNames = monthNames.ToArray(),
                AbbreviatedMonthGenitiveNames = monthNames.ToArray(),
                AMDesignator = "قبل‌ازظهر",
                PMDesignator = "بعدازظهر",
                FirstDayOfWeek = DayOfWeek.Saturday,
                FullDateTimePattern = "yyyy MMMM dddd, dd HH:mm:ss",
                LongDatePattern = "yyyy MMMM dddd, dd",
                ShortDatePattern = "yyyy/MM/dd"
            };
        }

        private static NumberFormatInfo GetCultureNumberFormat(string[] nativeDigits)
        {
            return new NumberFormatInfo
            {
                NativeDigits = nativeDigits.ToArray(),
                CurrencyDecimalSeparator = ",",
                NumberDecimalSeparator = "/",
                CurrencySymbol = "ریال"
            };
        }
    }
}
