using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var type = value.GetType();

            if (type.IsEnum)
                return Enum.IsDefined(type, value);

            if (value is string str && AllowEmptyStrings == false)
                return String.IsNullOrWhiteSpace(str) == false;

            if (AllowDefaultValues == false)
                return value.Equals(type.DefaultValue()) == false;

            return true;
        }
    }
}
