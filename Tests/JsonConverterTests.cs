using System;
using Model;
using NodaTime;
using NUnit.Framework;
using Period = Model.Period;

namespace Tests
{
    [TestFixture]
    public class JsonConverterTests
    {
        [Test]
        public void CanDeserializeSearchWindow()
        {
            var parser = new JsonConverter<SearchWindow>();
            var result = parser.ConvertFrom(@"{
            Period: {
                From: '2012-4-5',
                To: '2013-4-4'
            },

            TimeFrame: {
                FromDay: 'Monday',
                ToDay: 'Friday',
                FromTime: '08:00:00',
                ToTime: '18:00:00'
            }
        }");
            Assert.AreEqual(new SearchWindow(new TimeFrame(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8,0), new LocalTime(18,0)),
                new Period(new DateTime(2012,4,5),new DateTime(2013,4,4) ) ), 
                result);
        }      
        
        [Test]
        public void CanDeserializeTimeFrame()
        {
            var parser = new JsonConverter<TimeFrame>();
            var result = parser.ConvertFrom(@"{
                FromDay: 'Monday',
                ToDay: 'Friday',
                FromTime: '08:00:00',
                ToTime: '18:00:00'
            }");
            Assert.AreEqual(new TimeFrame(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8,0),new LocalTime(18,0)), result);
        }      
        
        [Test]
        public void CanDeserializePeriod()
        {
            var parser = new JsonConverter<Period>();
            var result = parser.ConvertFrom(@"{
                From: '2012-4-5',
                To: '2013-4-4'
            }");
            Assert.AreEqual(new Period(new DateTime(2012,4,5),new DateTime(2013, 4, 4) ), result);
        }
    }
}
