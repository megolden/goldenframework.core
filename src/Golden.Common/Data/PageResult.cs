using System.Collections.Generic;

namespace Golden.Common.Data
{
    public class PageResult<T>
    {
        public IEnumerable<T> Elements { get; private set; }
        public int TotalElements { get; private set; }

        public PageResult(IEnumerable<T> elements, int totalElements)
        {
            Elements = elements;
            TotalElements = totalElements;
        }
    }
}
