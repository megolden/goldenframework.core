using System;
using System.Collections.Generic;

namespace Golden.Common.Persian
{
    public static class NumberPersianUtils
    {
        private const string SEPARATOR = " و ";
        private const string NEGATE_SYMBOL = "منفی";

        private static readonly string[] UNITS =
        {
            "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه",
            "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده"
        };
        private static readonly string[] TENS =
        {
            "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود"
        };
        private static readonly string[] HUNDREDS =
        {
            "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد"
        };
        private static readonly string[] THOUSANDS =
        {
            "هزار", "میلیون", "میلیارد", "تریلیون", "کوآدریلیون", "کوینتیلیون"
        };

        public static string NumberToLetter(long number)
        {
            return NumberToLetter(number, 0);
        }

        private static string NumberToLetter(long number, int thousandPower)
        {
            var resultParts = new List<string>();

            var isNegate = number < 0;
            var negatePrefix = isNegate ? NEGATE_SYMBOL + " ": String.Empty;
            if (isNegate) number = -number;

            if (number == 0)
            {
                return thousandPower == 0 ? UNITS[0] : String.Empty;
            }

            if (number == 1)
            {
                if (thousandPower == 1) return negatePrefix + THOUSANDS[0];
            }

            for (var legalThousandPower = THOUSANDS.Length; legalThousandPower > 0; legalThousandPower--)
            {
                var n = (long)Math.Pow(1000D, legalThousandPower);
                if (number >= n)
                {
                    resultParts.Add(NumberToLetter(number / n, thousandPower + legalThousandPower));
                    number %= n;
                }
            }

            if (number >= 100)
            {
                resultParts.Add(HUNDREDS[number / 100 - 1]);
                number %= 100;
            }

            if (number > 19)
            {
                resultParts.Add(TENS[number / 10 - 2]);
                number %= 10;
            }

            if (number > 0)
            {
                resultParts.Add(UNITS[number]);
            }

            if (thousandPower > 0)
            {
                resultParts[^1] += " " + THOUSANDS[thousandPower - 1];
            }

            return negatePrefix + resultParts.Join(SEPARATOR);
        }
    }
}
