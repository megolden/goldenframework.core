using System;
using System.Collections.Generic;

namespace Golden.Common
{
    public static class ListUtils
    {
        public static void Swap<T>(this IList<T> list, int firstItemIndex, int secondItemIndex)
        {
            if (firstItemIndex == secondItemIndex) return;

            var firstItem = list[firstItemIndex];
            list[firstItemIndex] = list[secondItemIndex];
            list[secondItemIndex] = firstItem;
        }
    }
}
