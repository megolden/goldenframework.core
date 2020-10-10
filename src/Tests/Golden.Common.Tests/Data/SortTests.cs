using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Golden.Common.Data;
using Xunit;
using static Golden.Common.Data.SortDirection;

namespace Golden.Common.Tests.Data
{
    public class SortTests
    {
        [Fact]
        void Constructor_constructs_a_Sort_properly()
        {
            var sort = Sort<Book>.By("Id", Ascending);

            sort.Orders.Should().HaveCount(1);
            sort.Orders.Single().Property.Name.Should().Be("Id");
            sort.Orders.Single().Direction.Should().Be(Ascending);
        }

        [Fact]
        void And_append_new_property_to_existing_sort()
        {
            var existingSort = Sort<Book>.By("Id", Ascending);

            var sort = existingSort.And(Sort<Book>.By("Name", Descending));

            sort.Orders.Should().HaveCount(2);
            sort.Orders.First().Property.Name.Should().Be("Id");
            sort.Orders.First().Direction.Should().Be(Ascending);
            sort.Orders.Last().Property.Name.Should().Be("Name");
            sort.Orders.Last().Direction.Should().Be(Descending);
        }

        [Fact]
        void And_appends_duplicate_existing_property()
        {
            var existingSort = Sort<Book>.By("Id", Ascending);

            var sort = existingSort.And(Sort<Book>.By("Id", Descending));

            sort.Orders.Should().HaveCount(2);
            sort.Orders.ElementAt(0).Property.Name.Should().Be("Id");
            sort.Orders.ElementAt(0).Direction.Should().Be(Ascending);
            sort.Orders.ElementAt(1).Property.Name.Should().Be("Id");
            sort.Orders.ElementAt(1).Direction.Should().Be(Descending);
        }

        [Fact]
        void Sort_sorts_collection_in_ascending_order()
        {
            var list = new List<Book>
            {
                new Book { Id = 1, Title = "A" },
                new Book { Id = 3, Title = "C" },
                new Book { Id = 2, Title = "B" },
                new Book { Id = 0, Title = "G" }

            }.AsQueryable();
            var sort = Sort<Book>.By("Id", Ascending);
            var expectedSortedList = new List<Book>
            {
                new Book { Id = 0, Title = "G" },
                new Book { Id = 1, Title = "A" },
                new Book { Id = 2, Title = "B" },
                new Book { Id = 3, Title = "C" }
            };

            var actualSortedlist = list.Sort(sort);

            actualSortedlist.Should().BeEquivalentTo(expectedSortedList);
        }

        [Fact]
        void Sort_sorts_collection_in_descending_order()
        {
            var list = new List<Book>
            {
                new Book { Id = 1, Title = "A" },
                new Book { Id = 3, Title = "C" },
                new Book { Id = 2, Title = "B" },
                new Book { Id = 0, Title = "G" }

            }.AsQueryable();
            var sort = Sort<Book>.By("Title", Descending);
            var expectedSortedList = new List<Book>
            {
                new Book { Id = 0, Title = "G" },
                new Book { Id = 3, Title = "C" },
                new Book { Id = 2, Title = "B" },
                new Book { Id = 1, Title = "A" }
            };

            var actualSortedlist = list.Sort(sort);

            actualSortedlist.Should().BeEquivalentTo(expectedSortedList);
        }

        [Fact]
        void Sort_sorts_collection_by_multiple_properties()
        {
            var list = new List<Book>
            {
                new Book { Id = 1, Title = "A" },
                new Book { Id = 3, Title = "C" },
                new Book { Id = 4, Title = "G" },
                new Book { Id = 2, Title = "B" },
                new Book { Id = 5, Title = "G" }

            }.AsQueryable();
            var sort = Sort<Book>.By("Title", Ascending).And(Sort<Book>.By("Id", Descending));
            var expectedSortedList = new List<Book>
            {
                new Book { Id = 1, Title = "A" },
                new Book { Id = 2, Title = "B" },
                new Book { Id = 3, Title = "C" },
                new Book { Id = 5, Title = "G" },
                new Book { Id = 4, Title = "G" }
            };

            var actualSortedlist = list.Sort(sort);

            actualSortedlist.Should().BeEquivalentTo(expectedSortedList);
        }

        [Fact]
        void By_creates_sort_by_lambda_property()
        {
            var sort = Sort<Book>.By(_ => _.Id, Ascending);

            sort.Orders.Should().HaveCount(1);
            sort.Orders.Single().Property.Name.Should().Be("Id");
            sort.Orders.Single().Direction.Should().Be(Ascending);
        }

        [Fact]
        void By_creates_sort_with_default_ascending_order()
        {
            var sort = Sort<Book>.By(_ => _.Id);

            sort.Orders.Should().HaveCount(1);
            sort.Orders.Single().Property.Name.Should().Be("Id");
            sort.Orders.Single().Direction.Should().Be(Ascending);
        }

        [Fact]
        void Ascending_change_order_of_properties_to_ascending_order()
        {
            var sort = Sort<Book>.By("Id", Descending).Ascending();

            sort.Orders.Should().HaveCount(1);
            sort.Orders.First().Property.Name.Should().Be("Id");
            sort.Orders.First().Direction.Should().Be(Ascending);
        }

        [Fact]
        void Descending_change_order_of_properties_to_descending_order()
        {
            var sort = Sort<Book>.By("Id", Ascending).Descending();

            sort.Orders.Should().HaveCount(1);
            sort.Orders.First().Property.Name.Should().Be("Id");
            sort.Orders.First().Direction.Should().Be(Descending);
        }

        [Fact]
        void By_accepts_public_field()
        {
            var sort = Sort<Book>.By("Name");

            sort.Orders.Should().HaveCount(1);
            sort.Orders.Single().Property.Name.Should().Be("Name");
        }

        [Fact]
        void By_fails_when_non_public_property_is_passed()
        {
            Action by = () => Sort<Book>.By("Age");

            by.Should().Throw<Exception>();
        }

        [Fact]
        void By_find_properties_in_case_insensitive_strategy()
        {
            var sort = Sort<Book>.By("id");

            sort.Orders.Should().HaveCount(1);
            sort.Orders.Single().Property.Name.Should().Be("Id");
        }

        class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Name;
            private int Age { get; set; }
        }
    }
}
