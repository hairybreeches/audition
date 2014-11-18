﻿using System;
using Model.SearchWindows;
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
                () => new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Saturday, new LocalTime(16, 0), new LocalTime(15, 0)));
        }
    }
}