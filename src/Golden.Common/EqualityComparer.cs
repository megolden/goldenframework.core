using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Golden.Common
{
    public class EqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
        private static readonly MethodInfo _getHashCodeMethod;

        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int>? _fnGetHashCode;

        static EqualityComparer()
        {
            _getHashCodeMethod = typeof(Object).GetMethod(
                nameof(Object.GetHashCode),
                BindingFlags.Public | BindingFlags.Instance);
        }

        public EqualityComparer(Func<T, T, bool> comparer)
        {
            _comparer = comparer;
        }

        public EqualityComparer(Func<T, T, bool> comparer, Func<T, int> fnGetHashCode) : this(comparer)
        {
            _fnGetHashCode = fnGetHashCode;
        }

        public virtual bool Equals(T obj, T anotherObj)
        {
            if (obj.IsNull()) return anotherObj.IsNull();
            if (anotherObj.IsNull()) return obj.IsNull();
            return _comparer(obj, anotherObj);
        }
        bool IEqualityComparer.Equals(object obj, object anotherObj)
        {
            if (obj is T tObj && anotherObj is T tAnotherObj)
                return Equals(tObj, tAnotherObj);
            else
                return false;
        }

        public virtual int GetHashCode(T obj)
        {
            if (_fnGetHashCode != null)
            {
                return _fnGetHashCode(obj);
            }
            else
            {
                if (obj.IsNotNull())
                    return obj.GetHashCode();
                else
                    return 0;
            }
        }
        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj.IsNull()) return 0;

            if (obj is T tObj)
                return GetHashCode(tObj);
            else
                throw new InvalidOperationException();
        }

        public static EqualityComparer<T> By<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var type = typeof(T);
            var propertyExpr = ExpressionUtils.GetMember(property);

            // Comparer
            var xParam = Expression.Parameter(type, "x");
            var yParam = Expression.Parameter(type, "y");
            var xProperty = Expression.MakeMemberAccess(xParam, propertyExpr);
            var yProperty = Expression.MakeMemberAccess(yParam, propertyExpr);
            var equalsMethod = Expression.Equal(xProperty, yProperty);
            var comparerFunction = Expression.Lambda<Func<T, T, bool>>(equalsMethod, xParam, yParam).Compile();

            // GetHashCode
            var objParam = Expression.Parameter(type, "obj");
            var objProperty = Expression.MakeMemberAccess(objParam, propertyExpr);
            var getHashCodeMethodCall = Expression.Call(objProperty, _getHashCodeMethod);
            var getHashCodeFunction = Expression.Lambda<Func<T, int>>(getHashCodeMethodCall, objParam).Compile();

            var comparerType = typeof(EqualityComparer<>).MakeGenericType(type);
            var parameters = new object[] { comparerFunction, getHashCodeFunction };
            return (EqualityComparer<T>)Activator.CreateInstance(comparerType, parameters);
        }
    }
}
