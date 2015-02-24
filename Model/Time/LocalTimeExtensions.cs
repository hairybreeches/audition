using System.Globalization;
using NodaTime;

namespace Model.Time
{
    public static class LocalTimeExtensions
    {
        public static string ToShortString(this LocalTime time)
        {
            return string.Format(time.ToString("H:mm", new DateTimeFormatInfo()));
        }
    }
}
