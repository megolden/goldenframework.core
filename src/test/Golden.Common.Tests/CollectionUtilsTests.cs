using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class CollectionUtilsTests
    {
        [Fact]
        void AssignNewItems_assigns_new_items_to_collection()
        {
            var collection = new List<int> { 1, 2 };
            var newItems = new[] { 5, 6 };

            collection.AssignNewItems(newItems);

            collection.Should().HaveCount(2);
            collection.Should().Contain(newItems);
            collection.Should().NotContain(new[] { 1, 2 });
        }
    }
}
