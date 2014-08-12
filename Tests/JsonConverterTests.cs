using System;
using Model;
using NodaTime;
using NUnit.Framework;

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
                From: '5/4/2013',
                To: '4/4/2013'
            },

            TimeFrame: {
                FromDay: 'Monday',
                ToDay: 'Friday',
                FromTime: '08:00',
                ToTime: '18:00'
            }
        }");
            Assert.AreEqual(DayOfWeek.Wednesday, result);
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
    }
}
