using System;
using System.ComponentModel;
using System.Globalization;

namespace Golden.Common.Persian
{
    public class PersianDateTimeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            var srcType = Nullable.GetUnderlyingType(sourceType) ?? sourceType;

            if (srcType == typeof(PersianDateTime)) return true;
            if (srcType == typeof(DateTime)) return true;
            if (srcType == typeof(string)) return true;

            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            var desType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

            if (desType == typeof(PersianDateTime)) return true;
            if (desType == typeof(DateTime)) return true;
            if (desType == typeof(string)) return true;

            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is null) return null;

            if (value is PersianDateTime)
                return value;

            if (value is DateTime d)
                return PersianDateTime.FromDateTime(d);

            if (value is string s)
                return PersianDateTime.Parse(s);

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is null) return null;


            if (value is IConvertible convertible)
            {
                var desType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
                return convertible.ToType(desType, culture);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (value is PersianDateTime)
                return true;

            if (value is DateTime)
                return true;

            if (value is string s)
                return PersianDateTime.TryParse(s, out _);

            return false;
        }
    }
}
