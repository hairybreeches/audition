using System;
using NodaTime;

namespace Model
{
    public class TimeFrame
    {
        public TimeFrame(DayOfWeek fromDay, DayOfWeek toDay, LocalTime fromTime, LocalTime toTime)
        {
            ToTime = toTime;
            FromTime = fromTime;
            ToDay = toDay;
            FromDay = fromDay;
        }

        public DayOfWeek FromDay { get; private set; }
        public DayOfWeek ToDay { get; private set; }
        public LocalTime FromTime { get; private set; }
        public LocalTime ToTime { get; private set; }
    }
}