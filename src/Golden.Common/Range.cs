using System;

namespace Golden.Common
{
    public class Range<T> where T : IComparable<T>
    {
        public T Min { get; private set; }

        public T Max { get; private set; }

        public Range(T min, T max)
        {
            if (min.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            Min = min;
            Max = max;
        }

        public bool Includes(T value)
        {
            return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
        }
    }
}
