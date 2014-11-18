using System;

namespace Model
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset ToUkDateTimeOffsetFromUtc(this DateTime dateTime)
        {
            var dateUtc = new DateTimeOffset(dateTime, TimeSpan.Zero);
            var offset = GetUkOffset(dateUtc);
            return dateUtc.ToOffset(offset);
        }

        private static TimeSpan GetUkOffset(DateTimeOffset dateUtc)
        {
            var timezone = GetUkTimezone();
            return timezone.GetUtcOffset(dateUtc);
        }

        private static TimeZoneInfo GetUkTimezone()
        {
            return TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        }
    }
}