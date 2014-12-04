using System;
using Audition.Chromium;
using Autofac;
using Model;
using Model.SearchWindows;
using Model.Time;
using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;
using Webapp.Requests;

namespace Tests
{
    [TestFixture]
    public class JsonConverterTests
    {
        [Test]
        public void CanDeserializeSearchWindow()
        {
            var result = Parse<SearchWindow<WorkingHoursParameters>>(@"{
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

            Assert.AreEqual(new SearchWindow<WorkingHoursParameters>(new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8, 0), new LocalTime(18, 0)),
                new DateRange(new DateTime(2012,4,5),new DateTime(2013,4,4) )), 
                result);
        }      
        
        [Test]
        public void CanDeserializeAccountsSearchWindow()
        {
            var result = Parse<SearchWindow<UnusualAccountsParameters>>(@"{
            Period: {
                From: '2012-4-5',
                To: '2013-4-4'
            },

            Parameters: {
                MinimumEntriesToBeConsideredNormal: 2
            }
        }");

            Assert.AreEqual(new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(2012, 4, 5), new DateTime(2013, 4, 4))), 
                result);
        } 
        
        [Test]
        public void CanDeserializeSearchRequest()
        {
            var result = Parse<SearchRequest<UnusualAccountsParameters>>(@"{
            pageNumber: 7,
            searchWindow: {
                Period: {
                    From: '2012-4-5',
                    To: '2013-4-4'
                },

                Parameters: {
                    MinimumEntriesToBeConsideredNormal: 2
                }
        }}");

            Assert.AreEqual(new SearchRequest<UnusualAccountsParameters>(new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(2012, 4, 5), new DateTime(2013, 4, 4))), 7), 
                result);
        }

        [Test]
        public void CanDeserializeExportRequest()
        {
            var result = Parse<ExportRequest<UserParameters>>(@"{
            SearchWindow: {
                Period: {
                    From: '2012-4-5',
                    To: '2013-4-4'
                },

                Parameters: {
                    Users: 'steve
                                alf'
                }
            },

            SerialisationOptions:{
                showUsername: true,
                showDescription: false
            }

        }");
            Assert.AreEqual(new ExportRequest<UserParameters>(new SearchWindow<UserParameters>(new UserParameters("steve\nalf"), new DateRange(new DateTime(2012, 4, 5), new DateTime(2013, 4, 4))), new SerialisationOptions(true, false)), result);
        }          

       [Test]
       public void CanDeserializeTimeFrame()
       {           
           var result = Parse<WorkingHoursParameters>(@"{
               FromDay: 'Monday',
               ToDay: 'Friday',
               FromTime: '08:00',
               ToTime: '18:00'
           }");
           Assert.AreEqual(new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8,0),new LocalTime(18,0)), result);
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
