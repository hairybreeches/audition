using System;
using Audition.Chromium;
using Autofac;
using Model.Time;
using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;
using Webapp;
using Webapp.Requests;

namespace Tests
{
    [TestFixture]
    public class JsonConverterTests
    {        
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
            builder.RegisterModule<HttpModule>();
            using (var scope = builder.Build())
            {
                var settings = scope.Resolve<JsonSerializerSettings>();
                return JsonConvert.DeserializeObject<T>(value, settings);
            }
        }
    }
}
