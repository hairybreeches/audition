﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SearchWindows;
using Model.Time;
using NodaTime;
using NUnit.Framework;
using Xero;
using XeroApi.Model;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class DaylightSavingTests
    {


        [TestCaseSource("JournalsInside9To5")]
        public void SearcherMakesSureJournalsAreNotReturnedWhenTheyShouldntBe(Journal journalThatShouldBeReturned)
        {
            var resultsOfSearch = GetResultsOfSearching(journalThatShouldBeReturned);
            CollectionAssert.IsEmpty(resultsOfSearch, "This journal should not be returned");
        }
        
        [TestCaseSource("JournalsOutside9To5")]
        public void SearcherMakesSureJournalsAreReturnedWhenTheyShouldBe(Journal journalThatShouldBeReturned)
        {
            var resultsOfSearch = GetResultsOfSearching(journalThatShouldBeReturned);
            CollectionAssert.AreEquivalent(new []{journalThatShouldBeReturned.JournalID}, resultsOfSearch.Select(x=>x.Id), "This journal should be returned");

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

        private static IEnumerable<Model.Accounting.Journal> GetResultsOfSearching(Journal journal)
        {
            //a journal is unusual if and only if it is outside 9-5
            var hours = new WorkingHours(DayOfWeek.Monday, DayOfWeek.Sunday, new LocalTime(9, 0), new LocalTime(17, 0));
            var period = new DateRange(DateTime.MinValue, DateTime.MaxValue);
            var window = new SearchWindow<WorkingHours>(hours, period);

            var repository = new RepositoryWrapper(new []{journal});
            var searcher = new XeroJournalSearcher(repository);

            var resultsOfSearch = searcher.FindJournalsWithin(window);
            return resultsOfSearch;
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
