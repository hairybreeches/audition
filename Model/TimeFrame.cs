using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;

namespace Model
{
    public class TimeFrame
    {
        public TimeFrame(DayOfWeek fromDay, DayOfWeek toDay, LocalTime fromTime, LocalTime toTime)
        {
            if (toTime < fromTime)
            {
                throw new InvalidTimeFrameException("The 'from' time must be before the 'after' time");
            }
            ToTime = toTime;
            FromTime = fromTime;
            ToDay = toDay;
            FromDay = fromDay;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek FromDay { get; private set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek ToDay { get; private set; }
        public LocalTime FromTime { get; private set; }
        public LocalTime ToTime { get; private set; }

        public override string ToString()
        {
            return String.Format("From: {0} to {1}, {2} to {3}", FromDay, ToDay, FromTime, ToTime);
        }

        protected bool Equals(TimeFrame other)
        {
            return FromDay == other.FromDay && ToDay == other.ToDay && FromTime.Equals(other.FromTime) && ToTime.Equals(other.ToTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeFrame) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) FromDay;
                hashCode = (hashCode*397) ^ (int) ToDay;
                hashCode = (hashCode*397) ^ FromTime.GetHashCode();
                hashCode = (hashCode*397) ^ ToTime.GetHashCode();
                return hashCode;
            }
        }
    }
}