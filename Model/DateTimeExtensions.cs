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
        
        public static DateTimeOffset ToUkDateTimeOffsetFromUkLocalTime(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime, GetUkOffset(dateTime));
        }

        private static TimeSpan GetUkOffset(DateTimeOffset dateTimeOffset)
        {
            var timezone = GetUkTimezone();
            return timezone.GetUtcOffset(dateTimeOffset);
        }
        
        private static TimeSpan GetUkOffset(DateTime ukDateTime)
        {
            var timezone = GetUkTimezone();
            return timezone.GetUtcOffset(ukDateTime);
        }

        private static TimeZoneInfo GetUkTimezone()
        {
            return TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        }
    }
}