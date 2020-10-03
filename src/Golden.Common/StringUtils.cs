using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Golden.Common
{
    public static class StringUtils
    {
        public static string[] SplitLines(this string value, bool removeEmptyEntries = false)
        {
            var lines = value.Replace("\r\n", "\n").Replace("\n\r", "\n").Split('\n', '\r');

            if (removeEmptyEntries)
                lines = lines.Where(_ => _.Length > 0).ToArray();

            return lines;
        }

        public static string Left(this string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static string Repeat(this string str, int count)
        {
            return String.Concat(Enumerable.Repeat(str, count));
        }

        public static string Replace(this string str, int startIndex, int length, string newValue)
        {
            var result = new StringBuilder(str);
            result.Remove(startIndex, length);
            result.Insert(startIndex, newValue);
            return result.ToString();
        }

        public static string Reverse(this string str)
        {
            return String.Concat(Enumerable.Reverse(str));
        }

        public static string Concat(this string value, params string[] values)
        {
            return value.Concat(values.AsEnumerable());
        }
        public static string Concat(this string value, IEnumerable<string> values)
        {
            return String.Concat(values.Prepend(value));
        }

        public static bool IsMatch(this string value, string pattern, bool ignoreCase = true)
        {
            var options = RegexOptions.None;
            if (ignoreCase) options |= RegexOptions.IgnoreCase;
            return Regex.IsMatch(value, pattern, options);
        }

        public static string EncodeBase64(this string value)
        {
            return value.EncodeBase64(Encoding.UTF8);
        }
        public static string EncodeBase64(this string value, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(value));
        }

        public static string DecodeBase64(this string value)
        {
            return value.DecodeBase64(Encoding.UTF8);
        }
        public static string DecodeBase64(this string value, Encoding encoding)
        {
            var decodedBytes = Convert.FromBase64String(value);
            return encoding.GetString(decodedBytes);
        }

        public static string Format(this string value, object arguments)
        {
            if (String.IsNullOrEmpty(value)) return value;

            var buffer = new StringBuilder(value);
            var properties = arguments.GetType().GetProperties();
            foreach (var property in properties)
            {
                var name = $"{{{property.Name}}}";
                var argValue = property.GetValue(arguments);
                buffer.Replace(name, argValue.ToString());
            }
            return buffer.ToString();
        }

        public static byte[] GetBytes(this string value)
        {
            return value.GetBytes(Encoding.UTF8);
        }
        public static byte[] GetBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        public static bool EqualsOrdinal(this string value, string anotherValue, bool ignoreCase = false)
        {
            var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return String.Equals(value, anotherValue, comparisonType);
        }

        public static int CompareOrdinal(this string value, string anotherValue, bool ignoreCase = false)
        {
            var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return String.Compare(value, anotherValue, comparisonType);
        }

        public static string EmptyAsNull(this string value)
        {
            return String.IsNullOrEmpty(value) ? null : value;
        }

        public static string TrimToNull(this string value)
        {
            if (value == null) return null;

            var result = value.Trim();

            return result.Length > 0 ? result : null;
        }

        public static bool IsEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return String.IsNullOrEmpty(value) == false;
        }

        public static bool IsBlank(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotBlank(this string value)
        {
            return String.IsNullOrWhiteSpace(value) == false;
        }
    }
}
