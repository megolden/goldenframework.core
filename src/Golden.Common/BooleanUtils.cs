namespace Golden.Common
{
    public static class BooleanUtils
    {
        public static bool And(this bool condition, bool anotherCondition)
        {
            return condition && anotherCondition;
        }

        public static bool Or(this bool condition, bool anotherCondition)
        {
            return condition || anotherCondition;
        }

        public static bool AndNot(this bool condition, bool anotherCondition)
        {
            return condition && !anotherCondition;
        }

        public static bool OrNot(this bool condition, bool anotherCondition)
        {
            return condition || !anotherCondition;
        }
    }
}
