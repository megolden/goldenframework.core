using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class EnumerableUtilsTests
    {
        [Fact]
        void ForEach_invokes_an_action_on_collection_items()
        {
            var numbers = new[] { new Int(1), new Int(2), new Int(3) };
            var expectedNumbers = new[] { new Int(2), new Int(3), new Int(4) };

            numbers.ForEach(_ => _.IncreaseOne());

            numbers.Should().BeEquivalentTo(expectedNumbers);
        }

        [Fact]
        void ForEach_invokes_an_action_with_item_index_on_collection_items()
        {
            var numbers = new[] { 1, 2, 3 };
            var expectedIndexs = new[] { 0, 1, 2 };

            var actualIndexs = new List<int>();
            numbers.ForEach((_, index) => actualIndexs.Add(index));

            actualIndexs.Should().BeEquivalentTo(expectedIndexs);
        }

        [Fact]
        void AddAll_adds_all_specified_items_to_collection()
        {
            var numbers = new HashSet<int> { 1, 2, 3 };
            var newNumbers = new[] { 4, 5 };
            var expectedNumbers = new[] { 1, 2, 3, 4, 5 };

            numbers.AddAll(newNumbers);

            numbers.Should().BeEquivalentTo(expectedNumbers);
        }

        [Fact]
        void Exclude_returns_new_collection_without_specified_value()
        {
            var numbers = new[] { 1, 2, 3, 2, 5, 9 };
            var excludeValue = 2;
            var expectedNumbers = new[] { 1, 3, 5, 9 };

            var actualNumbers = numbers.Exclude(excludeValue);

            actualNumbers.Should().BeEquivalentTo(expectedNumbers);
        }

        [Fact]
        void Find_finds_first_item_matching_with_specified_predicate_in_collection()
        {
            var numbers = new[] { 1, 2, 3, 5 };
            Func<int, bool> searchPredicate = _ => _ == 2;
            var expectedValue = 2;

            var actualValue = numbers.Find(searchPredicate);

            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        void Find_returns_default_value_when_no_predicate_matching_item_found_in_collection()
        {
            var numbers = new int?[] { 1, 2, 3, 5 };
            Func<int?, bool> searchPredicate = _ => _ == 4;
            int? expectedValue = null;

            var actualValue = numbers.Find(searchPredicate);

            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        void DistinctBy_returns_distinct_elements_of_collection_comparing_by_specified_property()
        {
            var collection = new[] { new Int(1), new Int(3), new Int(5), new Int(1) };
            var expectedCollection = new[] { new Int(1), new Int(3), new Int(5) };

            var resultCollection = collection.DistinctBy(_ => _.Value);

            resultCollection.Should().BeEquivalentTo(expectedCollection);
        }

        [Fact]
        void IntersectBy_returns_intersection_elements_of_two_collection_comparing_by_specified_property()
        {
            var collection1 = new[] { new Int(1), new Int(3), new Int(5), new Int(10) };
            var collection2 = new[] { new Int(6), new Int(5), new Int(8), new Int(1) };
            var expectedCollection = new[] { new Int(1), new Int(5) };

            var resultCollection = collection1.IntersectBy(collection2, _ => _.Value);

            resultCollection.Should().BeEquivalentTo(expectedCollection);
        }

        [Fact]
        void UnionBy_returns_union_elements_of_two_collection_comparing_by_specified_property()
        {
            var collection1 = new[] { new Int(1), new Int(3), new Int(5) };
            var collection2 = new[] { new Int(2), new Int(5), new Int(4), new Int(1) };
            var expectedCollection = new[] { new Int(1), new Int(2), new Int(3), new Int(4), new Int(5) };

            var resultCollection = collection1.UnionBy(collection2, _ => _.Value);

            resultCollection.Should().BeEquivalentTo(expectedCollection);
        }

        [Fact]
        void ExceptBy_returns_difference_elements_of_two_collection_comparing_by_specified_property()
        {
            var collection1 = new[] { new Int(1), new Int(2), new Int(3), new Int(5) };
            var collection2 = new[] { new Int(2), new Int(5), new Int(4) };
            var expectedCollection = new[] { new Int(1), new Int(3) };

            var resultCollection = collection1.ExceptBy(collection2, _ => _.Value);

            resultCollection.Should().BeEquivalentTo(expectedCollection);
        }

        [Fact]
        void Exists_returns_true_when_at_least_one_element_found_in_collection_with_matching_specified_condition()
        {
            var collection = new[] { 1, 2, 3, 5 };

            var result = collection.Exists(_ => _ % 2 == 0);

            result.Should().BeTrue();
        }

        [Fact]
        void Exists_returns_false_when_no_element_found_in_collection_with_matching_specified_condition()
        {
            var collection = new[] { 1, 2, 3, 5 };

            var result = collection.Exists(_ => _ == 4);

            result.Should().BeFalse();
        }

        [Fact]
        void NotExists_returns_true_when_no_element_found_in_collection_with_matching_specified_condition()
        {
            var collection = new[] { 1, 2, 3, 5 };

            var result = collection.NotExists(_ => _ == 4);

            result.Should().BeTrue();
        }

        [Fact]
        void NotExists_returns_false_when_at_least_one_element_found_in_collection_with_matching_specified_condition()
        {
            var collection = new[] { 1, 2, 3 };

            var result = collection.NotExists(_ => _ == 2);

            result.Should().BeFalse();
        }

        [Fact]
        void NotAll_returns_true_when_at_least_one_element_found_in_collection_that_not_matched_with_specified_condition()
        {
            var collection = new[] { 4, 2, 1 };

            var result = collection.NotAll(_ => _ % 2 == 0);

            result.Should().BeTrue();
        }

        [Fact]
        void NotAll_returns_false_when_all_elements_of_collection_matched_with_specified_condition()
        {
            var collection = new[] { 6, 2, 4 };

            var result = collection.NotAll(_ => _ % 2 == 0);

            result.Should().BeFalse();
        }

        [Fact]
        void IsEmpty_returns_true_when_collection_has_no_elements()
        {
            var collection = Enumerable.Empty<int>();

            var result = collection.IsEmpty();

            result.Should().BeTrue();
        }

        [Fact]
        void IsEmpty_returns_false_when_at_least_one_element_found_in_collection()
        {
            var collection = new[] { 1 };

            var result = collection.IsEmpty();

            result.Should().BeFalse();
        }

        [Fact]
        void IsNotEmpty_returns_true_when_at_least_one_element_found_in_collection()
        {
            var collection = new[] { 1 };

            var result = collection.IsNotEmpty();

            result.Should().BeTrue();
        }

        [Fact]
        void IsNotEmpty_returns_false_when_collection_has_no_elements()
        {
            var collection = Enumerable.Empty<int>();

            var result = collection.IsNotEmpty();

            result.Should().BeFalse();
        }

        [Fact]
        void Page_returns_page_of_elements_with_specified_PageNumber_and_PageSize()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var pageNumber = 2;
            var pageSize = 3;
            var expectedPage = new[] { 4, 5, 6 };

            var result = collection.Page(pageNumber, pageSize);

            result.Should().BeEquivalentTo(expectedPage);
        }

        [Fact]
        void RandomElement_returns_random_element_of_collection()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var result = collection.RandomElement();

            result.Should().BeOneOf(collection);
        }

        [Fact]
        void Join_concatenates_collection_elements_using_ToString_method_of_each_item()
        {
            var collection = new object[] { "ab", 2, 3.5, 'c' };
            var expectedResult = "ab23.5c";

            var result = collection.Join();

            result.Should().Be(expectedResult);
        }

        [Fact]
        void Join_concatenates_collection_elements_using_ToString_method_of_each_item_with_specified_separator()
        {
            var collection = new object[] { "ab", 2, 3.5, 'c' };
            var separator = "*";
            var expectedResult = "ab*2*3.5*c";

            var result = collection.Join(separator);

            result.Should().Be(expectedResult);
        }
    }

    class Int
    {
        public int Value;

        public Int(int value)
        {
            Value = value;
        }

        public void IncreaseOne() => Value++;

        public override bool Equals(object? obj) => Value == (obj as Int)?.Value;

        public void ChangeValue(int value) => Value = value;
    }
}
