﻿using System;
using System.Linq;
using Model.Accounting;
using Model.Searching;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    public class UserSearcherTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateTime InPeriod = YearEnd.Subtract(TimeSpan.FromDays(30));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        [Test]
        public void DoesNotReturnJournalsWhichDoNotApplyToTheFinancialPeriod()
        {
            var journalApplyingToPostYearEnd = CreateJournalByUser("steve", YearEnd.AddDays(1));
            var journalApplyingToPreYearstart = CreateJournalByUser("steve", YearStart.Subtract(TimeSpan.FromDays(1)));

            var searcher = CreateSearcher(journalApplyingToPostYearEnd, journalApplyingToPreYearstart);
            var result = searcher.FindJournalsWithin(new SearchWindow<UserParameters>(new UserParameters("nonexistent"), FinancialPeriod));
            CollectionAssert.IsEmpty(result, "Neither of the journals should be returned since they're outside the period");
        }

        private static IJournalSearcher<UserParameters> CreateSearcher(params Journal[] journals)
        {
            return new UserSearcher(new JournalRepository(journals));
        }

        private static Journal CreateJournalInPeriodByUser(string user)
        {
            return CreateJournalByUser(user, InPeriod);
        }

        private static Journal CreateJournalByUser(string user, DateTime journalDate)
        {
            return new Journal(Guid.NewGuid().ToString(), new DateTimeOffset(), journalDate, user, String.Empty, Enumerable.Empty<JournalLine>());
        }
    }
}
