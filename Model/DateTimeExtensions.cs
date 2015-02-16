using System;

namespace Model
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset ToUkDateTimeOffsetFromUkLocalTime(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime, GetUkOffset(dateTime));
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