using System.Collections.Generic;

namespace Golden.Common
{
    public static class CollectionUtils
    {
        public static void AssignNewItems<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            collection.Clear();
            collection.AddAll(newItems);
        }
    }
}
