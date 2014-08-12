using System;
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

        public DayOfWeek FromDay { get; private set; }
        public DayOfWeek ToDay { get; private set; }
        public LocalTime FromTime { get; private set; }
        public LocalTime ToTime { get; private set; }

        public override string ToString()
        {
            return String.Format("From: {0} to {1}, {2} to {3}", FromDay, ToDay, FromTime, ToTime);
        }
    }
}