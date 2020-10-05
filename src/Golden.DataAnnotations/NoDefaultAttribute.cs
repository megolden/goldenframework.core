using System;
using System.ComponentModel.DataAnnotations;
using Golden.Common;

namespace Golden.DataAnnotations
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false,
        Inherited = false)]
    public class NoDefaultAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var type = value.GetType();

            return value.Equals(type.DefaultValue()) == false;
        }
    }
}
