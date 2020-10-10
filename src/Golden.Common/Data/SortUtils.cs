using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Golden.Common.Data
{
    public static class SortUtils
    {
        private static readonly Lazy<MethodInfo> _queryableOrderByMethod;

        private static readonly Lazy<MethodInfo> _queryableThenByMethod;

        private static readonly Lazy<MethodInfo> _queryableOrderByDescendingMethod;

        private static readonly Lazy<MethodInfo> _queryableThenByDescendingMethod;

        static SortUtils()
        {
            _queryableThenByDescendingMethod = new Lazy<MethodInfo>(()
                => ResolveQueryableMethod(nameof(Queryable.ThenByDescending), parameterCount: 2));

            _queryableOrderByDescendingMethod = new Lazy<MethodInfo>(()
                => ResolveQueryableMethod(nameof(Queryable.OrderByDescending), parameterCount: 2));

            _queryableThenByMethod = new Lazy<MethodInfo>(()
                => ResolveQueryableMethod(nameof(Queryable.ThenBy), parameterCount: 2));

            _queryableOrderByMethod = new Lazy<MethodInfo>(()
                => ResolveQueryableMethod(nameof(Queryable.OrderBy), parameterCount: 2));
        }

        private static MethodInfo ResolveQueryableMethod(string name, int parameterCount)
        {
            var searchFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;
            return typeof(Queryable)
                .GetMember(name, searchFlags)
                .OfType<MethodInfo>()
                .Single(_ => _.GetParameters().Length == parameterCount);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, Sort<T> sort)
        {
            var query = source;

            var sortOrders = sort.Orders;
            for (var propertyIndex = 0; propertyIndex < sortOrders.Length; propertyIndex++)
            {
                var sortOrder = sortOrders[propertyIndex];
                var secondLevel = propertyIndex > 0;
                query = SortSingleProperty(query, sortOrder.Property, sortOrder.Direction, secondLevel);
            }

            return query;
        }

        private static IQueryable<T> SortSingleProperty<T>(
            IQueryable<T> query,
            MemberInfo member,
            SortDirection sortDirection,
            bool isSecondLevel)
        {
            var sourceType = typeof(T);
            var parameter = Expression.Parameter(sourceType, name: "_");
            var propertyExpr = Expression.MakeMemberAccess(parameter, member);
            var lambdaPropertyExpr = Expression.Lambda(propertyExpr, parameter);

            var sortMethod = (sortDirection, isSecondLevel) switch
            {
                (SortDirection.Ascending, Item2: false) => _queryableOrderByMethod.Value,
                (SortDirection.Ascending, Item2: true) => _queryableThenByMethod.Value,
                (SortDirection.Descending, Item2: false) => _queryableOrderByDescendingMethod.Value,
                (SortDirection.Descending, Item2: true) => _queryableThenByDescendingMethod.Value,
            };

            query = (IQueryable<T>)sortMethod
                .MakeGenericMethod(sourceType, member.GetMemberType())
                .Invoke(null, new object[] { query, lambdaPropertyExpr });

            return query;
        }
    }
}
