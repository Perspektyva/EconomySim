namespace World
{
    public struct IntRange
    {
        private int lowerInclusive;
        private int upperInclusive;

        private IntRange(int lowerInclusive, int upperInclusive)
        {
            this.lowerInclusive = lowerInclusive;
            this.upperInclusive = upperInclusive;
        }

        public static IntRange Inclusive(int lower, int upper)
            => new IntRange(lower, upper);

        public bool Contains(int value)
            => value >= this.lowerInclusive && value <= this.upperInclusive;
    }
}
