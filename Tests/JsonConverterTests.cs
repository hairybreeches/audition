using System;
using Audition.Chromium;
using Autofac;
using Model;
using Newtonsoft.Json;
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
            var result = Parse<HoursSearchWindow>(@"{
            Period: {
                From: '2012-4-5',
                To: '2013-4-4'
            },

            Parameters: {
                FromDay: 'Monday',
                ToDay: 'Friday',
                FromTime: '08:00',
                ToTime: '18:00'
            }
        }");      
      
            Assert.AreEqual(new HoursSearchWindow(new TimeFrame(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8,0), new LocalTime(18,0)),
                new DateRange(new DateTime(2012,4,5),new DateTime(2013,4,4) ) ), 
                result);
        }

        
       [Test]
       public void CanDeserializeTimeFrame()
       {           
           var result = Parse<TimeFrame>(@"{
               FromDay: 'Monday',
               ToDay: 'Friday',
               FromTime: '08:00',
               ToTime: '18:00'
           }");
           Assert.AreEqual(new TimeFrame(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8,0),new LocalTime(18,0)), result);
       }      
        
       [Test]
       public void CanDeserializePeriod()
       {           
           var result = Parse<DateRange>(@"{
               From: '2012-4-5',
               To: '2013-4-4'
           }");
           Assert.AreEqual(new DateRange(new DateTime(2012,4,5),new DateTime(2013, 4, 4) ), result);
       }

        private static T Parse<T>(string value)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ChromiumModule>();
            using (var scope = builder.Build())
            {
                var settings = scope.Resolve<JsonSerializerSettings>();
                return JsonConvert.DeserializeObject<T>(value, settings);
            }
        }
    }
}
