using System;
using System.ComponentModel.DataAnnotations;
using Golden.Common;

namespace Golden.DataAnnotations
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false,
        Inherited = false)]
    public class RequiredAttribute : ValidationAttribute
    {
        public bool AllowEmptyStrings { get; set; } = false;
        public bool AllowDefaultValues { get; set; } = true;

        public override bool IsValid(object? value)
        {
            if (value is null) return false;

            var type = value.GetType();

            if (type.IsEnum)
                return Enum.IsDefined(type, value);

            if (value is string str && !AllowEmptyStrings)
                return !String.IsNullOrWhiteSpace(str);

            if (!AllowDefaultValues)
                return !value.Equals(type.DefaultValue());

            return true;
        }
    }
}
