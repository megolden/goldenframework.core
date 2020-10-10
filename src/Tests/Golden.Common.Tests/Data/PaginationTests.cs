using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Golden.Common.Data;
using Xunit;

namespace Golden.Common.Tests.Data
{
    public class PaginationTests
    {
        [Fact]
        public void Of_creates_a_Pagination_properly()
        {
            var pageable = Pagination.Of(1, 10);

            pageable.PageNumber.Should().Be(1);
            pageable.PageSize.Should().Be(10);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Of_fails_when_invlaid_pageNumber_passed(int pageNumber)
        {
            Action of = () => Pagination.Of(pageNumber, 1);

            of.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Of_fails_when_invlaid_pageSize_passed(int pageSize)
        {
            Action of = () => Pagination.Of(1, pageSize);

            of.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void PageResult_returns_middle_page_of_collection()
        {
            var list = new List<Book>
            {
                new Book(1, "A"),
                new Book(2, "B"),
                new Book(3, "C"),
                new Book(4, "D"),
                new Book(5, "E"),
                new Book(6, "F"),
                new Book(7, "G")

            }.AsQueryable();
            var pageNumber = 2;
            var pageSize = 3;
            var expectedPage = new List<Book>
            {
                new Book(4, "D"),
                new Book(5, "E"),
                new Book(6, "F")
            };

            var page = list.PageResult(Pagination.Of(pageNumber, pageSize));

            page.TotalElements.Should().Be(list.Count());
            page.Elements.Should().BeEquivalentTo(expectedPage);
        }

        class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }

            public Book(int id, string title)
            {
                Id = id;
                Title = title;
            }
        }
    }
}
