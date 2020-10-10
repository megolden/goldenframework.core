using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Golden.Common.Data
{
    public class Sort<T>
    {
        private readonly List<SortOrder> _orders;

        public SortOrder[] Orders => _orders.ToArray();

        private Sort(IEnumerable<SortOrder> orders)
        {
            _orders = orders.ToList();
        }

        public Sort<T> And(Sort<T> sort)
        {
            return new Sort<T>(_orders.Concat(sort._orders));
        }

        public Sort<T> Ascending()
        {
            return new Sort<T>(_orders.Select(_ => new SortOrder(_.Property, SortDirection.Ascending)));
        }

        public Sort<T> Descending()
        {
            return new Sort<T>(_orders.Select(_ => new SortOrder(_.Property, SortDirection.Descending)));
        }

        public static Sort<T> By<TProperty>(Expression<Func<T, TProperty>> property)
            => By(property, SortDirection.Ascending);
        public static Sort<T> By<TProperty>(Expression<Func<T, TProperty>> property, SortDirection direction)
        {
            var memberExpression = property.Body.UnwrapQuote() as MemberExpression;
            return new Sort<T>(new[] { new SortOrder(memberExpression.Member, direction) });
        }

        public static Sort<T> By(string propertyName)
            => By(propertyName, SortDirection.Ascending);
        public static Sort<T> By(string propertyName, SortDirection direction)
        {
            var property = ResolveProperty(propertyName);
            return new Sort<T>(new[] { new SortOrder(property, direction) });
        }

        private static MemberInfo ResolveProperty(string propertyName)
        {
            var type = typeof(T);
            var searchFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
            MemberInfo member = type.GetProperty(propertyName, searchFlags);
            if (member == null) member = type.GetField(propertyName, searchFlags);

            if (member == null)
                throw new InvalidOperationException();

            return member;
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class SortOrder
    {
        public MemberInfo Property { get; private set; }
        public SortDirection Direction { get; private set; }

        public SortOrder(MemberInfo property, SortDirection direction)
        {
            Property = property;
            Direction = direction;
        }
    }
}
