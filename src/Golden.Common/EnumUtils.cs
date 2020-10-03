using System;
using System.Linq;
using System.Linq.Expressions;

namespace Golden.Common
{
    public static class EnumUtils
    {
        public static bool Has<T>(this T value, params T[] flags) where T : Enum
        {
            return flags.All(_ => value.HasFlag(_));
        }

        public static T Set<T>(this T value, params T[] flags) where T : Enum
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(T));

            var resultParam = Expression.Parameter(underlyingType, "result");
            var flagParam = Expression.Parameter(underlyingType, "flag");
            var bitwiseOrExpr = Expression.Or(resultParam, flagParam);
            var fnBitwiseOr = Expression.Lambda(bitwiseOrExpr, resultParam, flagParam).Compile();

            var result = Convert.ChangeType(value, underlyingType);
            flags.ForEach(flag => result = fnBitwiseOr.DynamicInvoke(result, flag));
            return (T)result;
        }

        public static T Unset<T>(this T value, params T[] flags) where T : Enum
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(T));

            var resultParam = Expression.Parameter(underlyingType, "result");
            var flagParam = Expression.Parameter(underlyingType, "flag");
            var bitwiseAndNotExpr = Expression.And(resultParam, Expression.Not(flagParam));
            var fnBitwiseOr = Expression.Lambda(bitwiseAndNotExpr, resultParam, flagParam).Compile();

            var result = Convert.ChangeType(value, underlyingType);
            flags.ForEach(flag => result = fnBitwiseOr.DynamicInvoke(result, flag));
            return (T)result;
        }

        public static bool HasAllFlags<T>(this T value) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return value.Has(values.OfType<T>().ToArray());
        }
    }
}
