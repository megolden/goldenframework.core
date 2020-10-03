using System;

namespace Golden.Common
{
    public static class IntegerUtils
    {
        public static int DivideRemainder(this int value, int divisor)
        {
            return value % divisor;
        }

        public static long DivideRemainder(this long value, long divisor)
        {
            return value % divisor;
        }

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        public static bool IsEven(this long value)
        {
            return value % 2L == 0L;
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        public static bool IsOdd(this long value)
        {
            return value % 2L != 0L;
        }
    }
}
