using System;
using System.Collections.Generic;
using System.Linq;
using Audition.Chromium;
using Audition.Requests;
using Audition.Session;
using Autofac;
using Model.Accounting;
using Model.Responses;
using Model.SearchWindows;
using Model.Time;
using Newtonsoft.Json;
using NUnit.Framework;
using Persistence;
using Sage50;

namespace SystemTests
{
    [TestFixture]
    public class PaginationTests
    {
        [Test]
        public void WhenWeGetALatePageNumberOfASearchWeGetTheRightResult()
        {
            var parameters = new EndingParameters(0);
            var searchRequest = new SearchRequest<EndingParameters>(new SearchWindow<EndingParameters>(parameters, new DateRange(DateTime.MinValue, DateTime.MaxValue)),149);
            var serialisedRequest = JsonConvert.SerializeObject(searchRequest);
            

            var journals = ExecuteSearch(serialisedRequest);

            var ids = journals.Select(x => x.Id);
            CollectionAssert.AreEqual(Enumerable.Range(1480, 10).Select(x => x.ToString()), ids);
        }

        private static IEnumerable<Journal> ExecuteSearch(string serialisedRequest)
        {
            var request = new MockRequestResponse("POST", serialisedRequest, "application/json",
                "http://localhost:1337/" + Routing.EndingSearch);

            var builder = SystemFoo.CreateDefaultContainerBuilder();
            using (var lifetime = builder.Build())
            {
                lifetime.Resolve<JournalRepository>().UpdateJournals(GetJournals());
                lifetime.Resolve<JournalSearcherFactoryStorage>().CurrentSearcherFactory = new Sage50SearcherFactory();
               return lifetime.GetParsedResponseContent<SearchResponse>(request).Journals;
            }
        }


        private static IEnumerable<Journal> GetJournals()
        {
            var startDate = new DateTimeOffset(new DateTime(1999, 1, 1), TimeSpan.Zero);
            for (var i = 0; i < 1500; i++)
            {
                yield return new Journal(i.ToString(), startDate.AddMinutes(5 * i), new DateTime(), "steve", "description", new[]
                {
                    new JournalLine("steve", "steve", JournalType.Dr, 20)                    
                });
            }
        }
    }
}
