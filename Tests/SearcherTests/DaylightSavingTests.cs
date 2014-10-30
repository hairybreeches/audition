using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Model.SearchWindows;
using Model.Time;
using NodaTime;
using NUnit.Framework;
using Tests.Mocks;
using Xero;
using XeroApi.Model;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class DaylightSavingTests
    {
        [Test]
        public void WhenGmtJournalsAreReturnedTheyHaveTheCorrectDateTimeOffset()
        {
            var gmtJournal = CreateXeroJournalFor(new DateTime(1999, 12, 21, 16, 0, 0));
            var resultsOfSearch = ResultsOfSearching(gmtJournal, new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Sunday, new LocalTime(11, 0), new LocalTime(13, 0)));
            var journalTime = resultsOfSearch.Single().Created;
            Assert.AreEqual(new DateTimeOffset(1999,12,21,16,0,0, TimeSpan.Zero), journalTime);
            Assert.AreEqual(TimeSpan.Zero, journalTime.Offset);
        }      
        
        
        [Test]
        public void WhenBstJournalsAreReturnedTheyHaveTheCorrectDateTimeOffset()
        {
            var gmtJournal = CreateXeroJournalFor(new DateTime(1999, 6, 21, 16, 0, 0));
            var resultsOfSearch = ResultsOfSearching(gmtJournal, new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Sunday, new LocalTime(11, 0), new LocalTime(13, 0)));
            var journalTime = resultsOfSearch.Single().Created;
            Assert.AreEqual(new DateTimeOffset(1999,6,21,17,0,0, TimeSpan.FromHours(1)), journalTime);
            Assert.AreEqual(TimeSpan.FromHours(1), journalTime.Offset);
        }

        [TestCaseSource("JournalsInside9To5")]
        public void SearcherMakesSureJournalsAreNotReturnedWhenTheyShouldntBeBasedOnTime(Journal journalThatShouldNotBeReturned)
        {
            var resultsOfSearch = ResultsOfSearching(journalThatShouldNotBeReturned, new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Sunday, new LocalTime(9, 0), new LocalTime(17, 0)));
            CollectionAssert.IsEmpty(resultsOfSearch, "This journal should not be returned");
        }
        
        [TestCaseSource("JournalsOutside9To5")]
        public void SearcherMakesSureJournalsAreReturnedWhenTheyShouldBeBasedOnTime(Journal journalThatShouldBeReturned)
        {
            var resultsOfSearch = ResultsOfSearching(journalThatShouldBeReturned, new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Sunday, new LocalTime(9, 0), new LocalTime(17, 0)));
            CollectionAssert.AreEquivalent(new []{journalThatShouldBeReturned.JournalID}, resultsOfSearch.Select(x=> new Guid(x.Id)), "This journal should be returned");
        }

        public IEnumerable<TestCaseData> JournalsInside9To5
        {
            get
            {
                //journals created between 9 and 5 when UK is on GMT
                yield return CreateTestCaseData(new DateTime(1999, 12, 21, 9, 30, 00), "GMT: journal after start of day not returned");
                yield return CreateTestCaseData(new DateTime(1999, 12, 21, 16, 30, 00), "GMT: journal before end of day not returned (fails when you treat GMT as BST)");
                //9 to 5 BST are the same as 8 to 4 UTC
                yield return CreateTestCaseData(new DateTime(1999, 6, 21, 8, 30, 00), "BST: journal after start of day not returned (fails when you treat BST as GMT)");
                yield return CreateTestCaseData(new DateTime(1999, 6, 21, 15, 30, 00), "BST: journal before end of day not returned");
            }

        }

        public IEnumerable<TestCaseData> JournalsOutside9To5
        {
            get
            {
                //journals created outside 9 to 5 when UK is on GMT
                yield return CreateTestCaseData(new DateTime(1999, 12, 21, 8, 30, 00), "GMT: journal before start of day returned (fails when you treat GMT as BST)");
                yield return CreateTestCaseData(new DateTime(1999, 12, 21, 17, 30, 00), "GMT: journal after end of day returned");
                //9 to 5 BST are the same as 8 to 4 UTC
                yield return CreateTestCaseData(new DateTime(1999, 6, 21, 7, 30, 00), "BST: journal before start of day returned");
                yield return CreateTestCaseData(new DateTime(1999, 6, 21, 16, 30, 00), "BST: journal after end of day returned (fails when you treat BST as GMT)");

            }
        }

        [TestCaseSource("JournalsInsideMonToFri")]
        public void SearcherMakesSureJournalsAreNotReturnedWhenTheyShouldntBeBasedOnDay(Journal journalThatShouldNotBeReturned)
        {
            var resultsOfSearch = ResultsOfSearching(journalThatShouldNotBeReturned, new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(0,0),new LocalTime(23,59) ));
            CollectionAssert.IsEmpty(resultsOfSearch, "This journal should not be returned");
        }  
        
        
        [TestCaseSource("JournalsOutsideMonToFri")]
        public void SearcherMakesSureJournalsAreReturnedWhenTheyShouldBeBasedOnDay(Journal journalThatShouldBeReturned)
        {
            var resultsOfSearch = ResultsOfSearching(journalThatShouldBeReturned, new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(0,0),new LocalTime(23,59) ));
            CollectionAssert.AreEquivalent(new []{journalThatShouldBeReturned.JournalID}, resultsOfSearch.Select(x=> new Guid(x.Id)), "This journal should be returned");
        }
        
        public IEnumerable<TestCaseData> JournalsInsideMonToFri
        {
            get
            {
                //Monday-Friday runs 15-19 Dec 2014
                yield return CreateTestCaseData(new DateTime(2014, 12, 15, 0, 30, 00), "GMT: journal after start of working week not returned");
                yield return CreateTestCaseData(new DateTime(2014, 12, 19, 23, 30, 00), "GMT: journal before end of working week not returned");
                //Monday-Friday runs 16-20 June 2014
                //BST is one hour in front, 
                yield return CreateTestCaseData(new DateTime(2014, 6, 15, 23, 30, 00), "BST: journal after start of working week not returned");
                yield return CreateTestCaseData(new DateTime(2014, 6, 20, 22, 30, 00), "BST: journal before end of working week not returned");
            }

        }
        
        public IEnumerable<TestCaseData> JournalsOutsideMonToFri
        {
            get
            {
                //Monday-Friday runs 15-19 Dec 2014
                yield return CreateTestCaseData(new DateTime(2014, 12, 14, 23, 30, 00), "GMT: journal before start of working week returned");
                yield return CreateTestCaseData(new DateTime(2014, 12, 20, 00, 30, 00), "GMT: journal after end of working week returned");
                //Monday-Friday runs 16-20 June 2014
                //BST is one hour in front, 
                yield return CreateTestCaseData(new DateTime(2014, 6, 15, 22, 30, 00), "BST: journal before start of working week returned");
                yield return CreateTestCaseData(new DateTime(2014, 6, 20, 23, 30, 00), "BST: journal after end of working week returned");
            }

        }

        [TestCaseSource("TimezoneSwitchovers")]
        public void UsesCorrectSwitchoverTimeForGmtToBst(DateTime utcTime, LocalTime ukTime)
        {
            var journal = CreateXeroJournalFor(utcTime);
            var windowContainingJournal = new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Sunday,
                ukTime.Minus(Period.FromMinutes(30)), ukTime.PlusMinutes(30));

            var results = ResultsOfSearching(journal, windowContainingJournal);

            //the window should contain the journal so should get no results
            CollectionAssert.IsEmpty(results);
        }


        public IEnumerable<TestCaseData> TimezoneSwitchovers
        {
            get
            {
                //In 2014, daylight savings starts at 1am on 30th March
                yield return new TestCaseData(new DateTime(2014, 3, 30, 0, 30, 0), new LocalTime(0, 30)).SetName("Before 1am UTC 30th March 2014 we are at UTC");
                yield return new TestCaseData(new DateTime(2014, 3, 30, 01, 30, 00), new LocalTime(2,30)).SetName("After 1am UTC 30th March 2014 we are at BST (+1:00)");                
                //in 2014, daylight savings ends at 2am BST (1am UTC/GMT) 26th October
                yield return new TestCaseData(new DateTime(2014, 10, 26, 00, 30, 00), new LocalTime(1, 30)).SetName("Before 2am BST (1am GMT) on 26th October we are at BST (+1:00)");
                yield return new TestCaseData(new DateTime(2014, 10, 26, 01, 30, 00), new LocalTime(01, 30)).SetName("After 1am GMT on 26th October we are at UTC");
            }

        }
        private static IEnumerable<Model.Accounting.Journal> ResultsOfSearching(Journal journal, WorkingHoursParameters workingHours)
        {
//a journal is unusual if and only if it is outside 9-5
            var hours = workingHours;
            var period = new DateRange(DateTime.MinValue, DateTime.MaxValue);
            var window = new SearchWindow<WorkingHoursParameters>(hours, period);


            var builder = new ContainerBuilder();
            builder.RegisterModule<XeroModule>();
            builder.Register(_ => new MockXeroSession(journal)).As<IXeroSession>();

            using (var lifetime = builder.Build())
            {
                var factory = lifetime.Resolve<XeroSearcherFactory>();
                var repoFactory = lifetime.Resolve<IXeroJournalGetter>();
                var searcher = factory.CreateJournalSearcher(repoFactory.CreateRepository("steve").Result);
                var resultsOfSearch = searcher.FindJournalsWithin(window);
                return resultsOfSearch;
            }
        }

        private static TestCaseData CreateTestCaseData(DateTime createdDateUtc, string name)
        {
            return new TestCaseData(CreateXeroJournalFor(createdDateUtc)).SetName(name);
        }

        private static Journal CreateXeroJournalFor(DateTime createdDateUtc)
        {
            return new Journal
            {
                JournalDate = new DateTime(1999, 3, 4),
                CreatedDateUTC = createdDateUtc,
                JournalLines = new JournalLines()
            };
        }
    }
}
