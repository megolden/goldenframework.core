using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Golden.Common.Persian
{
    public static class StringPersianUtils
    {
        public static string ToPersianDigits(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return value;

            var result = new StringBuilder();
            foreach (var character in value)
            {
                if (character >= '0' && character <= '9')
                    result.Append((char)(character + 1728));
                else
                    result.Append(character);
            }
            return result.ToString();
        }

        public static string ToLatinDigits(this string value, bool alsoArabicDigits = true)
        {
            if (String.IsNullOrWhiteSpace(value)) return value;

            var result = new StringBuilder();
            foreach (var character in value)
            {
                if (character >= 1776 && character <= 1785)
                    result.Append((char)(character - 1728));
                else if (alsoArabicDigits && character >= 1632 && character <= 1641)
                    result.Append((char)(character - 1584));
                else
                    result.Append(character);
            }
            return result.ToString();
        }

        public static string ToPersianKafYeh(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return value;

            var result = new StringBuilder();
            foreach (var character in value)
            {
                if (character == 'ك')
                    result.Append('ک');
                else if (character == 'ي')
                    result.Append('ی');
                else
                    result.Append(character);
            }
            return result.ToString();
        }
    }
}
