using System;

namespace Native.Time
{
    public class Clock : IClock
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Now.Date;
        }
    }
}