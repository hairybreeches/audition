using System;

namespace Model
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset ToUkDateTimeOffsetFromUtc(this DateTime dateTime)
        {
            var dateUtc = new DateTimeOffset(dateTime, TimeSpan.Zero);
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var offset = timezone.GetUtcOffset(dateUtc);
            var localDateTime = dateUtc.ToOffset(offset);
            return localDateTime;
        }               
    }
}