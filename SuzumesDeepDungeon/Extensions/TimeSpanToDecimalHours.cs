namespace SuzumesDeepDungeon.Extensions
{
    /// <summary>
    /// Extension methods for TimeSpan to convert to decimal hours.
    /// </summary>
    public static class TimeSpanExtensions
    {
        public static double ToDecimalHours(this TimeSpan timeSpan, int decimals = 1)
        {
            return Math.Round(timeSpan.TotalHours, decimals);
        }

        /// <summary>
        /// Converts a decimal hour value to a TimeSpan.
        /// </summary>
        /// <param name="decimalHours"></param>
        /// <returns></returns>
        public static TimeSpan DecimalHoursToTimeSpan(double decimalHours)
        {
            int hours = (int)decimalHours;
            double minutes = (decimalHours - hours) * 60;
            return new TimeSpan(hours, (int)minutes, 0);
        }
    }
}
