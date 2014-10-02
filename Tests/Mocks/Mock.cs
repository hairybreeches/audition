using System;
using System.Linq;
using Model;
using Model.Accounting;
using NodaTime;
using NUnit.Framework;
using Xero;

namespace Tests.Mocks
{
    public static class Mock
    {
        public static IJournalSearcher JournalSearcher(params Journal[] journals)
        {
            var factory = MockXeroRepositoryFactory.Create(journals);
            return new XeroJournalSearcher(factory);
        }

        public static Journal GetJournalPostedAt(LocalTime journalTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 23, journalTime.Hour, journalTime.Minute, journalTime.Second),
                new DateTime(2012,1,3), Enumerable.Empty<JournalLine>());
        }

        public static Journal GetJournalAffecting(DateTime dateTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 1),
                dateTime, Enumerable.Empty<JournalLine>());
        }

        public static Journal GetJournalPostedOn(DayOfWeek day)
        {
            var dayOfMonth = 6 + (int) day; //the 6th of July 2014 was a Sunday, Sunday is the 0th element of the enum.
            var journal = new Journal(Guid.NewGuid(), 
                new DateTime(2014, 7, dayOfMonth),
                new DateTime(), Enumerable.Empty<JournalLine>());

            Assert.AreEqual(day, journal.Created.DayOfWeek,
                "GetJournalPostedOn should return a journal posted on the right day of the week");
            
            return journal;
        }

        public static Journal JournalPostedTo(string accountCode1, string accountCode2)
        {
            return JournalPostedTo(accountCode1, accountCode2, new DateTime(1999, 12, 1));
        }

        public static Journal JournalPostedTo(string accountCode1, string accountCode2, DateTime journalDate)
        {
            return new Journal(Guid.NewGuid(), new DateTime(1999, 12, 1), journalDate,
                new[]
                {
                    new JournalLine(accountCode1, accountCode1, JournalType.Cr, 2.2m),
                    new JournalLine(accountCode2, accountCode2, JournalType.Dr, 2.2m)
                });
        }
    }
}