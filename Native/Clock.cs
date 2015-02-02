using System;

namespace Native
{
    public class Clock : IClock
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Now.Date;
        }
    }
}