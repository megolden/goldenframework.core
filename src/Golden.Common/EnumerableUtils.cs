using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Golden.Common
{
    public static class EnumerableUtils
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
            => collection.ForEach((item, _) => action(item));
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            using var enumerator = collection.GetEnumerator();
            var index = 0;
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, index);
                ++index;
            }
        }

        public static void AddAll<T>(this ICollection<T> collection, params T[] values)
            => collection.AddAll(values.AsEnumerable());
        public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            values.ForEach(collection.Add);
        }

        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> collection, T value)
        {
            using var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (Object.Equals(enumerator.Current, value) == false)
                    yield return enumerator.Current;
            }
        }

        public static T Find<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.FirstOrDefault(predicate);
        }

        public static IEnumerable<T> DistinctBy<T, TProperty>(
            this IEnumerable<T> collection,
            Expression<Func<T, TProperty>> property)
        {
            var comparer = EqualityComparer<T>.By(property);
            return collection.Distinct(comparer);
        }

        public static IEnumerable<T> IntersectBy<T, TProperty>(
            this IEnumerable<T> collection,
            IEnumerable<T> second,
            Expression<Func<T, TProperty>> property)
        {
            var comparer = EqualityComparer<T>.By(property);
            return collection.Intersect(second, comparer);
        }

        public static IEnumerable<T> UnionBy<T, TProperty>(
            this IEnumerable<T> collection,
            IEnumerable<T> second,
            Expression<Func<T, TProperty>> property)
        {
            var comparer = EqualityComparer<T>.By(property);
            return collection.Union(second, comparer);
        }

        public static IEnumerable<T> ExceptBy<T, TProperty>(
            this IEnumerable<T> collection,
            IEnumerable<T> second,
            Expression<Func<T, TProperty>> property)
        {
            var comparer = EqualityComparer<T>.By(property);
            return collection.Except(second, comparer);
        }

        public static bool Exists<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.Any(predicate);
        }

        public static bool NotExists<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.Any(predicate) == false;
        }

        public static bool NotAll<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.All(predicate) == false;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection.Any() == false;
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
        {
            return collection.Any();
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            if (pageNumber == 1)
            {
                return collection.Take(pageSize);
            }
            else
            {
                var skipItemCount = (pageNumber - 1) * pageSize;
                return collection.Skip(skipItemCount).Take(pageSize);
            }
        }

        public static T RandomElement<T>(this IEnumerable<T> collection)
        {
            var randomIndex = new Random().Next(collection.Count());
            return collection.ElementAt(randomIndex);
        }

        public static string Join<T>(this IEnumerable<T> collection)
            => collection.Join(String.Empty);
        public static string Join<T>(this IEnumerable<T> collection, string separator)
        {
            return String.Join(separator, collection);
        }

        public static IEnumerable<T> Of<T>(params T[] items)
        {
            return items;
        }
    }
}
