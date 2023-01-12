namespace Common.Converters
{
    public static class ConvertersExtensions
    {
        public static int InchToCm(this int value)
        {
            return ((float)value).InchToCm();
        }

        public static int InchToCm(this float value)
        {
            return (int)Math.Round(2.54 * value);
        }
    }
}
