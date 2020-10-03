using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Golden.Common
{
    public static class ExpressionUtils
    {
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> expression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var parameter = source.Parameters.First();
            var leftExp = source.Body.UnwrapQuote();
            var rightExp = ExpressionReplacer.Replace(
                expression.Body.UnwrapQuote(),
                expression.Parameters.First(),
                parameter);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(leftExp, rightExp), parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> expression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var parameter = source.Parameters.First();
            var leftExp = source.Body.UnwrapQuote();
            var rightExp = ExpressionReplacer.Replace(
                expression.Body.UnwrapQuote(),
                expression.Parameters.First(),
                parameter);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(leftExp, rightExp), parameter);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expression.Body.UnwrapQuote()),
                expression.Parameters.First());
        }

        public static MemberInfo GetMember(Expression member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            var expression = member.UnwrapQuote();
            if (expression is MemberExpression memberExpression)
                return memberExpression.Member;
            else if (expression.NodeType.IsIn(ExpressionType.Convert, ExpressionType.ConvertChecked))
                return GetMember(expression.As<UnaryExpression>().Operand);
            else if (expression is LambdaExpression lambdaExpression)
                return GetMember(lambdaExpression.Body);
            else
                throw new ArgumentException("invalid member lambda expression");
        }

        private static Expression UnwrapQuote(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.NodeType == ExpressionType.Quote)
                return expression.As<UnaryExpression>().Operand.UnwrapQuote();

            return expression;
        }
    }
}
