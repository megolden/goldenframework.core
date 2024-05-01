using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Golden.Common
{
    public static class ObjectUtils
    {
        public static bool IsNull(this object value)
        {
            return Object.ReferenceEquals(value, null);
        }

        public static bool IsNotNull(this object value)
        {
            return Object.ReferenceEquals(value, null) == false;
        }

        public static int GetHashCode(IEnumerable<object> values)
        {
            var hashcodes = values.Select(ComputeHashCode);

            const int seed = 17;
            const int prime = 23;
            return hashcodes.Aggregate(seed, (result, hash) => prime * result + hash);
        }

        private static int ComputeHashCode(object value)
        {
            if (value == null)
                return 0;
            else if (value is string str)
                return GetHashCode(str.OfType<object>());
            else
                return value.GetHashCode();
        }

        public static bool NotIn<T>(this T value, params T[] items)
            => value.NotIn(items.AsEnumerable());

        public static bool NotIn<T>(this T value, IEnumerable<T> items)
        {
            return items.Contains(value) == false;
        }

        public static bool IsIn<T>(this T value, params T[] items)
            => value.IsIn(items.AsEnumerable());

        public static bool IsIn<T>(this T value, IEnumerable<T> items)
        {
            return items.Contains(value);
        }

        public static bool IsInRange<T>(this T value, T start, T end) where T : IComparable<T>
            => value.IsInRange(start, end, inclusive: true);

        public static bool IsInRange<T>(this T value, T start, T end, bool inclusive) where T : IComparable<T>
        {
            if (inclusive)
                return value.CompareTo(start) >= 0 && value.CompareTo(end) <= 0;
            else
                return value.CompareTo(start) >= 0 && value.CompareTo(end) < 0;
        }

        public static T As<T>(this object value)
        {
            return value is T to ? to : default(T);
        }

        public static bool Is<T>(this object value)
        {
            return value is T;
        }

        public static bool NotEqualTo(this object value, object anotherValue)
        {
            return value.Equals(anotherValue) == false;
        }

        public static void SetValue(this object obj, string name, object value)
        {
            var searchOptions = BindingFlags.Public |
                                BindingFlags.NonPublic |
                                BindingFlags.Instance |
                                BindingFlags.Static |
                                BindingFlags.IgnoreCase;

            var member = obj.GetType().GetMember(name, searchOptions)[0];
            member.SetMemberValue(value, obj);
        }

        public static void SetValue(this object obj, object memberValues)
        {
            var properties = memberValues.GetType().GetProperties();
            properties.ForEach(property => obj.SetValue(property.Name, property.GetValue(memberValues)));
        }

        public static T GetValue<T>(this object obj, string name)
            => (T)obj.GetValue(name);

        public static object GetValue(this object obj, string name)
        {
            var searchOptions = BindingFlags.Public |
                                BindingFlags.NonPublic |
                                BindingFlags.Instance |
                                BindingFlags.Static |
                                BindingFlags.IgnoreCase;

            var member = obj.GetType().GetMember(name, searchOptions)[0];
            return member.GetMemberValue(obj);
        }

        public static T With<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}
