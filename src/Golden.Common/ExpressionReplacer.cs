using System;
using System.Linq.Expressions;

namespace Golden.Common
{
    internal class ExpressionReplacer : ExpressionVisitor
    {
        private readonly Expression _oldExpression;
        private readonly Expression _newExpression;

        private ExpressionReplacer(Expression oldExpression, Expression newExpression)
        {
            _oldExpression = oldExpression;
            _newExpression = newExpression;
        }

        public override Expression Visit(Expression expression)
        {
            return expression == _oldExpression ? _newExpression : base.Visit(expression);
        }

        public static Expression Replace(
            Expression sourceExpression,
            Expression oldExpression,
            Expression newExpression)
        {
            return new ExpressionReplacer(oldExpression, newExpression).Visit(sourceExpression);
        }
    }
}
