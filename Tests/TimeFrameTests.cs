using System;
using Model;
using Model.Time;
using NodaTime;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TimeFrameTests
    {
        [Test]
        public void CannotCreateATimeFrameWithTimesWhichWrapAround()
        {
            Assert.Throws<InvalidTimeFrameException>(
                () => new TimeFrame(DayOfWeek.Monday, DayOfWeek.Saturday, new LocalTime(16, 0), new LocalTime(15, 0)));
        }
    }
}