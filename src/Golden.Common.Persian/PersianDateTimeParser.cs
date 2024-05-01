using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Golden.Common.Persian
{
    public class PersianDateTimeParser
    {
        private const string AltAMDesignator = "ق.ظ";
        private const string AltPMDesignator = "ب.ظ";
        private static readonly CultureInfo _culture;
        private static readonly Regex DATE_TIME_REGEX_PATTERN;

        static PersianDateTimeParser()
        {
            _culture = PersianCulture.Culture;

            var CULTURE_DATE_SEP_PATTERN = Regex.Escape(_culture.DateTimeFormat.DateSeparator);
            var CULTURE_TIME_SEP_PATTERN = Regex.Escape(_culture.DateTimeFormat.TimeSeparator);

            var HOUR_DESIGNATOR_PATTERN =
                "(" +
                Regex.Escape(_culture.DateTimeFormat.AMDesignator) + "|" +
                Regex.Escape(AltAMDesignator) + "|" +
                Regex.Escape(_culture.DateTimeFormat.PMDesignator) + "|" +
                Regex.Escape(AltPMDesignator) +
                ")";

            var DATE_SEP_PATTERN = @" \s* (/|-|\.|,|\s|" + CULTURE_DATE_SEP_PATTERN + @") \s* ";
            var TIME_SEP_PATTERN = @" \s* (:|\.|,|\s|" + CULTURE_TIME_SEP_PATTERN + @") \s* ";
            var DATE_TIME_SEP_PATTERN = @"(\s+|T)";

            var DATE_PATTERN =
                @$"(?<year>\d{{1,4}}) {DATE_SEP_PATTERN} (?<month>\d{{1,2}}) {DATE_SEP_PATTERN} (?<day>\d{{1,2}})";
            var DATE_PATTERN_ALT =
                @"(?<year>\d{4}) (?<month>\d{2}) (?<day>\d{2})";

            var TIME_PATTERN =
                @$"(?<hour>\d{{1,2}}) ({TIME_SEP_PATTERN} (?<min>\d{{1,2}}) ({TIME_SEP_PATTERN} (?<sec>\d{{1,2}}) ({TIME_SEP_PATTERN} (?<mill>\d{{1,3}}))? )? )? (\s+(?<des>{HOUR_DESIGNATOR_PATTERN}))?";

            var TIME_PATTERN_ALT =
                @"(?<hour>\d{2}) (?<min>\d{2}) (?<sec>\d{2}) (?<mill>\d{3})";

            var DATE_TIME_PATTERN =
                @$"({DATE_PATTERN} ({DATE_TIME_SEP_PATTERN} {TIME_PATTERN})? | {TIME_PATTERN})";
            var DATE_TIME_PATTERN_ALT =
                @$"{DATE_PATTERN_ALT} T? ({TIME_PATTERN_ALT})?";

            var ALL_DATE_TIME_PATTERN = @$"^ \s* (({DATE_TIME_PATTERN}) | ({DATE_TIME_PATTERN_ALT})) \s* $";

            DATE_TIME_REGEX_PATTERN = new Regex(
                ALL_DATE_TIME_PATTERN,
                RegexOptions.IgnorePatternWhitespace |
                RegexOptions.Singleline |
                RegexOptions.IgnoreCase |
                RegexOptions.Compiled);
        }

        public PersianDateTime Parse(string input)
        {
            if (TryParse(input, out var date) is false)
                throw new ArgumentException("Invalid input string date value.", nameof(input));

            return date;
        }

        private bool IsPMDesignator(string designator)
        {
            if (_culture.DateTimeFormat.PMDesignator.EqualsOrdinal(designator, ignoreCase: true))
                return true;
            if (AltPMDesignator.EqualsOrdinal(designator, ignoreCase: true))
                return true;
            return false;
        }

        public bool TryParse(string input, out PersianDateTime date)
        {
            date = default;

            var match = DATE_TIME_REGEX_PATTERN.Match(input);

            if (match.Success is false)
                return false;

            int? year = null, month = null, day = null;
            int? hour = null, minute = null, second = null, millisecond = null;
            string? des = null;
            Group g;

            g = match.Groups["year"];
            if (g.Success) year = int.Parse(g.Value, _culture);
            g = match.Groups["month"];
            if (g.Success) month = int.Parse(g.Value, _culture);
            g = match.Groups["day"];
            if (g.Success) day = int.Parse(g.Value, _culture);
            g = match.Groups["hour"];
            if (g.Success) hour = int.Parse(g.Value, _culture);
            g = match.Groups["min"];
            if (g.Success) minute = int.Parse(g.Value, _culture);
            g = match.Groups["sec"];
            if (g.Success) second = int.Parse(g.Value, _culture);
            g = match.Groups["mill"];
            if (g.Success) millisecond = int.Parse(g.Value, _culture);
            g = match.Groups["des"];
            if (g.Success) des = g.Value;

            var today = PersianDateTime.Today;
            if (year is null) year = today.Year;
            if (month is null) month = today.Month;
            if (day is null) day = today.Day;
            if (hour is null) hour = 0;
            if (minute is null) minute = 0;
            if (second is null) second = 0;
            if (millisecond is null) millisecond = 0;
            if (des is not null)
            {
                if (IsPMDesignator(des))
                    if (hour > 0 && hour < 12) hour = hour + 12;
            }

            var isValid = PersianDateTime.IsValid(
                year.Value, month.Value, day.Value,
                hour.Value, minute.Value, second.Value, millisecond.Value);

            if (isValid)
            {
                date = new PersianDateTime(
                    year.Value, month.Value, day.Value,
                    hour.Value, minute.Value, second.Value, millisecond.Value);
            }

            return isValid;
        }
    }
}
