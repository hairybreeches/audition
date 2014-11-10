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
using Native;
using NodaTime;
using NSubstitute;
using NUnit.Framework;
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
            builder.Register(_ => Substitute.For<IFileSystem>()).As<IFileSystem>();


            var requestData =
                new ExportRequest<EndingParameters>(
                    new SearchWindow<EndingParameters>(new EndingParameters(0),
                        new DateRange(DateTime.MinValue, DateTime.MaxValue)), new SerialisationOptions(true, true));

            using (var lifetime = builder.BuildSearchable(GetJournals()))
            using (var requestScope = lifetime.BeginRequestScope())
            {
                requestScope.Resolve<ExportController>().EndingExport(requestData).Wait();
                var ids = exporter.WrittenJournals.Select(x => x.Id);
                CollectionAssert.AreEqual(Enumerable.Range(0, 1500).Select(x => x.ToString()), ids);
            }           
        }

        //todo: surely these can somehow be done as test cases?
        [Test]
        public void PaginationWorksForEndingSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new EndingParameters(0)), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1480, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForAccountsSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new UnusualAccountsParameters(1000000000)), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1480, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForYearEndSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new YearEndParameters((DateTime.MaxValue - DateTime.MinValue).Days)), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1480, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForUserSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new UserParameters("a non-existent user")), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1480, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }
        
        [Test]
        public void PaginationWorksForWorkingHoursSearch()
        {
            var journals = Searching.ExecuteSearch(CreateSearchRequest(new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Monday, new LocalTime(7, 32), new LocalTime(7, 32))), GetJournals());
            CollectionAssert.AreEqual(Enumerable.Range(1480, 10).Select(x => x.ToString()), journals.Select(x=>x.Id));
        }

        public SearchRequest<T> CreateSearchRequest<T>(T searchParameters)
        {
            return new SearchRequest<T>(new SearchWindow<T>(searchParameters, new DateRange(DateTime.MinValue, DateTime.MaxValue)), 149);
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
