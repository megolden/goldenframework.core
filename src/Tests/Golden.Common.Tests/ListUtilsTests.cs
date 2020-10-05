using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class ListUtilsTests
    {
        [Fact]
        void Swap_swaps_two_items_in_list_with_specified_indexes()
        {
            var list = new List<int> { 1, 3, 2 };
            var expectedList = new[] { 1, 2, 3 };

            list.Swap(1, 2);

            list.Should().BeEquivalentTo(expectedList, _ => _.WithStrictOrdering());
        }
    }
}
