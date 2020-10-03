using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class ExpressionUtilsTests
    {
        [Fact]
        void Or_combines_two_expressions_with_OR_logic()
        {
            Expression<Func<int, bool>> even = number => number % 2 == 0;
            var numbers = List(-3, 1, -7, 2, 3, 4, 5);
            var expectedNumbers = List(-3, -7, 2, 4);

            var evenOrNegative = even.Or(number => number < 0);

            numbers.Where(evenOrNegative).Should().BeEquivalentTo(expectedNumbers);
        }

        [Fact]
        void And_combines_two_expressions_with_AND_logic()
        {
            Expression<Func<int, bool>> even = number => number % 2 == 0;
            var numbers = List(-3, 1, -7, 2, 3, 4, 5);
            var expectedNumbers = List(2, 4);

            var evenAndPositive = even.And(number => number > 0);

            numbers.Where(evenAndPositive).Should().BeEquivalentTo(expectedNumbers);
        }

        [Fact]
        void Not_applies_logical_NOT_on_expression()
        {
            Expression<Func<int, bool>> even = number => number % 2 == 0;
            var numbers = List(1, 2, 3, 4, 5);
            var expectedNumbers = List(1, 3, 5);

            var notEven = even.Not();

            numbers.Where(notEven).Should().BeEquivalentTo(expectedNumbers);
        }

        [Fact]
        void GetMember_returns_member_from_MemberAccess_expression()
        {
            var propertyExpr = Expression.Property(Expression.Parameter(typeof(Book)), "Code");
            var expectedMember = typeof(Book).GetProperty("Code");

            var member = ExpressionUtils.GetMember(propertyExpr);

            member.Should().Be(expectedMember);
        }

        [Fact]
        void GetMember_returns_member_from_quoted_MemberAccess_expression()
        {
            var propertyLambda = Expression.Lambda(Expression.Property(Expression.Parameter(typeof(Book)), "Code"));
            var quotedProperty = Expression.Quote(propertyLambda);
            var expectedMember = typeof(Book).GetProperty("Code");

            var member = ExpressionUtils.GetMember(quotedProperty);

            member.Should().Be(expectedMember);
        }

        [Fact]
        void GetMember_returns_member_from_convert_expression()
        {
            var propertyExpr = Expression.Convert(
                Expression.Property(Expression.Parameter(typeof(Book)), "Code"),
                typeof(Object));
            var expectedMember = typeof(Book).GetProperty("Code");

            var member = ExpressionUtils.GetMember(propertyExpr);

            member.Should().Be(expectedMember);
        }

        [Fact]
        void GetMember_returns_member_from_lambda_expression()
        {
            Expression<Func<Book, int>> propertyExpr = book => book.Code;
            var expectedMember = typeof(Book).GetProperty("Code");

            var member = ExpressionUtils.GetMember(propertyExpr);

            member.Should().Be(expectedMember);
        }

        private IQueryable<T> List<T>(params T[] items)
        {
            return items.AsQueryable();
        }

        class Book
        {
            public int Code { get; set; }
        }
    }
}
