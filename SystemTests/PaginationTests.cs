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
using NodaTime;
using NUnit.Framework;
using Persistence;
using Sage50;

namespace SystemTests
{
    [TestFixture]
    public class PaginationTests
    {
        [TestCaseSource("SearchRequests")]
        public void WhenWeGetALatePageNumberOfASearchWeGetTheRightResult(string requestData, string route)
        {
            var journals = ExecuteSearch(requestData, route);
            var ids = journals.Select(x => x.Id);
            CollectionAssert.AreEqual(Enumerable.Range(1480, 10).Select(x => x.ToString()), ids);
        }

        public IEnumerable<TestCaseData> SearchRequests
        {
            get
            {
                yield return CreateTestCaseData(new EndingParameters(0), Routing.EndingSearch, "Round number ending search");
                yield return CreateTestCaseData(new UnusualAccountsParameters(1000000000), Routing.AccountsSearch, "Unusual accounts search");                
                yield return CreateTestCaseData(new YearEndParameters((DateTime.MaxValue - DateTime.MinValue).Days), Routing.DateSearch, "Year end journals search");
                yield return CreateTestCaseDataFromSerialised("{users: 'non-existent user'}", Routing.UserSearch, "Unusual users search");
                yield return CreateTestCaseDataFromSerialised("{FromDay: 'Monday',ToDay: 'Monday',FromTime: '07:32',ToTime: '07:32'}", Routing.HoursSearch, "Working hours search");
            }
        }

        private static TestCaseData CreateTestCaseDataFromSerialised(string parameters, string route, string name)
        {
            var serialisedData = String.Format(@"{{searchWindow: {{parameters: {0}, period: {1}}}, pageNumber: {2}}}", parameters, JsonConvert.SerializeObject(new DateRange(DateTime.MinValue, DateTime.MaxValue)), 149);

            return new TestCaseData(serialisedData, route).SetName(name);
        }

        public TestCaseData CreateTestCaseData<T>(T searchParameters, string route, string name)
        {
            var searchRequest = new SearchRequest<T>(new SearchWindow<T>(searchParameters, new DateRange(DateTime.MinValue, DateTime.MaxValue)), 149);
            var serialisedRequest = JsonConvert.SerializeObject(searchRequest);

            return new TestCaseData(serialisedRequest, route).SetName(name);
        }

        private static IEnumerable<Journal> ExecuteSearch(string serialisedRequest, string route)
        {
            var request = new MockRequestResponse("POST", serialisedRequest, "application/json",
                "http://localhost:1337/" + route);

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
