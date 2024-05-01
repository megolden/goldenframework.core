using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Golden.Common.Persian
{
    public class PersianDateTimeFormatter
    {
        public string Format(PersianDateTime date, string? format)
        {
            if (format is null) format = "G";

			var culture = PersianCulture.Culture;

            var result = new StringBuilder();

            #region Standard

            if (format.Length == 1)
            {
                format = culture.DateTimeFormat.GetAllDateTimePatterns(format[0]).FirstOrDefault() ?? format;
            }

			#endregion

			#region Custom

			var fmtBuffer = new StringBuilder();
			//The DFA-Machine with five states ;-)
			//	0: Processing a character,
			//	1: Literal processing(single qute),
			//	2: Literal processing(double qute),
			//	3: Processing escape character
			//	4: Processing character as custom datetime format
			byte state = 0;
			int iCh = 0;
			char ch;
			#region fnProcessBuffer
			Action fnProcessBuffer = () =>
			{
				if (fmtBuffer.Length > 0)
				{
					result.Append(GetCustomizedFormat(ref date, fmtBuffer.ToString(), culture));
					fmtBuffer.Clear();
				}
			};
			#endregion
			while (iCh < format.Length)
			{
				ch = format[iCh];
				switch (state)
				{
					case 0:
						#region State 0
						if (ch == '\'')
						{
							fnProcessBuffer();
							state = 1;
						}
						else if (ch == '"')
						{
							fnProcessBuffer();
							state = 2;
						}
						else if (ch == '\\')
						{
							fnProcessBuffer();
							state = 3;
						}
						else if (ch == '%')
						{
							fnProcessBuffer();
							state = 4;
						}
						else
						{
							fmtBuffer.Append(ch);
						}
						#endregion
						break;
					case 1:
						#region State 1
						if (ch == '\'')
						{
							state = 0;
						}
						else
						{
							result.Append(ch);
						}
						#endregion
						break;
					case 2:
						#region State 2
						if (ch == '"')
						{
							state = 0;
						}
						else
						{
							result.Append(ch);
						}
						#endregion
						break;
					case 3:
						#region State 3
						result.Append(ch);
						state = 0;
						#endregion
						break;
					case 4:
						#region State 4
						result.Append(GetCustomizedFormat(ref date, ch.ToString(), culture));
						state = 0;
						#endregion
						break;
				}
				iCh++;
			}
			if (state != 0)
				throw new FormatException();
			else
				fnProcessBuffer();

			#endregion

			return result.ToString();
        }

        private static int FindDifferent(string s, int index)
		{
			if (index < 0 || string.IsNullOrEmpty(s) || index >= (s.Length - 1)) return -1;
			var ch = s[index];
			while (++index < s.Length)
			{
				if (s[index] != ch) return index;
			}
			return -1;
		}

		private static string GetCustomizedFormat(ref PersianDateTime time, string format, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(format)) return String.Empty;

			var result = new StringBuilder();

			#region fnFormat
			Action<string, PersianDateTime> fnFormat = (s, t) =>
			{
				#region d
				if ("d".EqualsOrdinal(s))
					result.Append(t.Day.ToString(culture));
				else if ("dd".EqualsOrdinal(s))
					result.Append(t.Day.ToString("00", culture));
				else if ("ddd".EqualsOrdinal(s))
					result.Append(culture.DateTimeFormat.AbbreviatedDayNames[(int)t.DayOfWeek]);
				else if ("dddd".EqualsOrdinal(s))
					result.Append(culture.DateTimeFormat.DayNames[(int)t.DayOfWeek]);
				#endregion
				#region f
				else if ("f".EqualsOrdinal(s))
					result.Append((t.Millisecond / 100).ToString(culture));
				else if ("ff".EqualsOrdinal(s))
					result.Append((t.Millisecond / 10).ToString(culture));
				else if ("fff".EqualsOrdinal(s))
					result.Append(t.Millisecond.ToString(culture));
				// else if ("ffff".EqualsOrdinal(s))
				// 	result.Append(t.Millisecond.ToString(culture));
				//else if (Utility.Utils.StringEquals(s, "fffff"))
				//	result.Append(t.Millisecond.ToString(culture));
				//else if (Utility.Utils.StringEquals(s, "ffffff"))
				//	result.Append(t.Millisecond.ToString(culture));
				//else if (Utility.Utils.StringEquals(s, "fffffff"))
				//	result.Append(t.Millisecond.ToString(culture));
				#endregion
				#region F
				else if ("F".EqualsOrdinal(s))
				{
					int temp = (t.Millisecond / 100);
					if (temp != 0) result.Append(temp.ToString(culture));
				}
				else if ("FF".EqualsOrdinal(s))
				{
					int temp = (t.Millisecond / 10);
					if (temp != 0) result.Append(temp.ToString(culture));
				}
				else if ("FFF".EqualsOrdinal(s))
				{
					int temp = (t.Millisecond / 100);
					if (temp != 0) result.Append(temp.ToString(culture));
				}
				//else if (Utility.Utils.StringEquals(s, "FFFF"))
				//{
				//	if (t.Millisecond != 0) result.Append(t.Millisecond.ToString(culture));
				//}
				//else if (Utility.Utils.StringEquals(s, "FFFFF"))
				//{
				//	if (t.Millisecond != 0) result.Append(t.Millisecond.ToString(culture));
				//}
				//else if (Utility.Utils.StringEquals(s, "FFFFFF"))
				//{
				//	if (t.Millisecond != 0) result.Append(t.Millisecond.ToString(culture));
				//}
				//else if (Utility.Utils.StringEquals(s, "FFFFFFF"))
				//{
				//	if (t.Millisecond != 0) result.Append(t.Millisecond.ToString(culture));
				//}
				#endregion
				#region g
				else if ("g".EqualsOrdinal(s))
				{
					// result.Append("");
				}
				else if ("gg".EqualsOrdinal(s))
				{
					// result.Append("");
				}
				#endregion
				#region h
				else if ("h".EqualsOrdinal(s))
					result.Append(t.Hour12.ToString(culture));
				else if ("hh".EqualsOrdinal(s))
					result.Append(t.Hour12.ToString("00", culture));
				#endregion
				#region H
				else if ("H".EqualsOrdinal(s))
					result.Append(t.Hour.ToString(culture));
				else if ("HH".EqualsOrdinal(s))
					result.Append(t.Hour.ToString("00", culture));
				#endregion
				#region K
				else if ("K".EqualsOrdinal(s))
                    // result.Append(string.Concat(BaseUtcTimeOffset.Hours.ToString("+00;-00;00", culture), culture.DateTimeFormat.TimeSeparator, BaseUtcTimeOffset.Minutes.ToString("00", culture)));
                    throw new NotImplementedException();
				#endregion
				#region m
				else if ("m".EqualsOrdinal(s))
					result.Append(t.Minute.ToString(culture));
				else if ("mm".EqualsOrdinal(s))
					result.Append(t.Minute.ToString("00", culture));
				#endregion
				#region M
				else if ("M".EqualsOrdinal(s))
					result.Append(t.Month.ToString(culture));
				else if ("MM".EqualsOrdinal(s))
					result.Append(t.Month.ToString("00", culture));
				else if ("MMM".EqualsOrdinal(s))
					result.Append(culture.DateTimeFormat.AbbreviatedMonthNames[t.Month - 1]);
				else if ("MMMM".EqualsOrdinal(s))
					result.Append(culture.DateTimeFormat.MonthNames[t.Month - 1]);
				#endregion
				#region s
				else if ("s".EqualsOrdinal(s))
					result.Append(t.Second.ToString(culture));
				else if ("ss".EqualsOrdinal(s))
					result.Append(t.Second.ToString("00", culture));
				#endregion
				#region t
				else if ("t".EqualsOrdinal(s))
					result.Append((t.IsAfternoon ? culture.DateTimeFormat.PMDesignator.Remove(1) : culture.DateTimeFormat.AMDesignator.Remove(1)));
				else if ("tt".EqualsOrdinal(s))
					result.Append((t.IsAfternoon ? culture.DateTimeFormat.PMDesignator : culture.DateTimeFormat.AMDesignator));
				#endregion
				#region y
				else if ("y".EqualsOrdinal(s))
					result.Append((t.Year % 100).ToString(culture));
				else if ("yy".EqualsOrdinal(s))
					result.Append((t.Year % 100).ToString("00", culture));
				else if ("yyy".EqualsOrdinal(s))
					result.Append((t.Year < 1000 ? t.Year.ToString("000", culture) : t.Year.ToString(culture)));
				else if ("yyyy".EqualsOrdinal(s))
					result.Append(t.Year.ToString("0000", culture));
				else if ("yyyyy".EqualsOrdinal(s))
					result.Append(t.Year.ToString("00000", culture));
				#endregion
				#region z
				else if ("z".EqualsOrdinal(s))
					// result.Append(BaseUtcTimeOffset.Hours.ToString(culture));
                    throw new NotImplementedException();
				else if ("zz".EqualsOrdinal(s))
					// result.Append(BaseUtcTimeOffset.Hours.ToString("00", culture));
                    throw new NotImplementedException();
				else if ("zzz".EqualsOrdinal(s))
					// result.Append(string.Concat(BaseUtcTimeOffset.Hours.ToString("00", culture), culture.DateTimeFormat.TimeSeparator, BaseUtcTimeOffset.Minutes.ToString("00", culture)));
                    throw new NotImplementedException();
				#endregion
				#region Separators
				else if (":".EqualsOrdinal(s))
					result.Append(culture.DateTimeFormat.TimeSeparator);
				else if ("/".EqualsOrdinal(s))
					result.Append(culture.DateTimeFormat.DateSeparator);
				#endregion
				else
					result.Append(s);
			};
			#endregion

			int i = 0, di = -1;
			while (i < format.Length)
			{
				di = FindDifferent(format, i);
				if (di == -1)
				{
					fnFormat(format.Substring(i), time);
					break;
				}
				else
				{
					fnFormat(format.Substring(i, di-i), time);
					i = di;
				}
			}

			return result.ToString();
		}

        private static CultureInfo GetCulture(IFormatProvider? formatProvider)
        {
            return formatProvider as CultureInfo ?? PersianCulture.Culture;
        }
    }
}
