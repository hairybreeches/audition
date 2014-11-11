using System;
using System.Collections.Generic;
using System.Linq;
using Audition.Controllers;
using Audition.Requests;
using Autofac;
using Excel;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NodaTime;
using NUnit.Framework;
using Persistence;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class PaginationTests
    {
        [Test]
        public void ExportsAllJournalsWhenRequested()
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder().SaveExportedFilesTo("steve");
            var exporter = new MockExporter();
            builder.Register(_ => exporter).As<IExcelExporter>();            


            var requestData =
                new ExportRequest<EndingParameters>(
                    new SearchWindow<EndingParameters>(new EndingParameters(0),
                        new DateRange(DateTime.MinValue, DateTime.MaxValue)), new SerialisationOptions(true, true));

            using (var lifetime = builder.BuildSearchable(GetJournals()))
            using (var requestScope = lifetime.BeginRequestScope())
            {
                requestScope.Resolve<ExportController>().EndingExport(requestData).Wait();
                var ids = exporter.WrittenJournals.Select(x => x.Id);
                CollectionAssert.AreEqual(Enumerable.Range(1, 3000).Select(x => x.ToString()), ids);
            }           
        }

        //todo: surely these can somehow be done as test cases?
        [Test]
        public void PaginationWorksForEndingSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new EndingParameters(0)), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1481, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForAccountsSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new UnusualAccountsParameters(1000000000)), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1481, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForYearEndSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new YearEndParameters((DateTime.MaxValue - DateTime.MinValue).Days)), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1481, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForUserSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new UserParameters("a non-existent user")), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1481, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForWorkingHoursSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Monday, new LocalTime(7, 32), new LocalTime(7, 32))), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1481, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }    
                
        
        [TestCase(15, 0, true, true, "15", -1, 0, ExpectedException = typeof(InvalidPageNumberException), TestName = "Requesting a page number < 1 gives correct exception")]
        [TestCase(15, 3, true, true, "15", -1,0, ExpectedException = typeof(InvalidPageNumberException), TestName = "Requesting a page number too large gives correct exception")]
        [TestCase(6, 1, false, false, "6", 1,6, TestName = "Less than one page of results means the only page has no next or previous")]
        [TestCase(10, 1, false, false, "10", 1,10, TestName = "Exactly one page of results means the only page has no next or previous")]
        [TestCase(40, 2, true, true, "40", 11,10, TestName = "A page in the middle should have a next and a previous page")]
        [TestCase(40, 4, true, false, "40", 31,10, TestName = "A full last page should have a previous but no next")]
        [TestCase(36, 4, true, false, "36", 31,6, TestName = "A half-full last page should have a previous but no next")]
        [TestCase(2560, 1, false, true, "more than 2000", 1,10, TestName = "First page of large result set displays correct results string")]
        [TestCase(2560, 5, true, true, "more than 2000", 41, 10, TestName = "Early middle page of large result set displays correct results string")]
        [TestCase(2560, 255, true, true, "more than 2000", 2541, 10, TestName = "Second last page of large result set displays correct results string")]
        [TestCase(2560, 256, true, false, "more than 2000", 2551, 10, TestName = "A full last page on a large results set should show correct results string")]
        [TestCase(2556, 256, true, false, "more than 2000", 2551, 6, TestName = "A half-full last page on a large results set should show correct results string")]
        public void NextAndPreviousButtonsAvailableWhenAppropriate(int numberOfResults, int pageNumber, bool previousPageShouldBeAvailable, bool nextPageShouldBeAvailable, string totalResults, int firstResult, int numberOfResultsReturned)
        {
            //given a search with some results
            var searchResults = GetJournals().Take(numberOfResults).AsQueryable();
            //when we send the pagination result back to the server
            var searchResponse = searchResults.GetPage(pageNumber);

            //Then the presence of a previous page should be as expected
            Assert.AreEqual(previousPageShouldBeAvailable, searchResponse.IsPreviousPage, "Response should know whether the page is the first page or not");
            Assert.AreEqual(nextPageShouldBeAvailable, searchResponse.IsNextPage, "Reponse should know whether the page is the last page or not.");
            Assert.AreEqual(totalResults, searchResponse.TotalResults, "Response should display the correct total number of results");
            Assert.AreEqual(firstResult, searchResponse.FirstResult, "Response should display correct index of first result");
            var expectedIds = Enumerable.Range(firstResult, numberOfResultsReturned).Select(x=>x.ToString());
            var actualIds = searchResponse.Journals.Select(x=>x.Id);
            Assert.AreEqual(expectedIds, actualIds, "Reponse should return correct results");
        }



        public SearchRequest<T> CreateSearchRequest<T>(T searchParameters)
        {
            return new SearchRequest<T>(new SearchWindow<T>(searchParameters, new DateRange(DateTime.MinValue, DateTime.MaxValue)), 149);
        }

        private static IEnumerable<Journal> GetJournals()
        {
            var startDate = new DateTimeOffset(new DateTime(1999, 1, 1), TimeSpan.Zero);
            for (var i = 1; i < 3001; i++)
            {
                yield return new Journal(i.ToString(), startDate.AddMinutes(5 * i), new DateTime(), "steve", "description", new[]
                {
                    new JournalLine("steve", "steve", JournalType.Dr, 20)                    
                });
            }
        }
    }
}
