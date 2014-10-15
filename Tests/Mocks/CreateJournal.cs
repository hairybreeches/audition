using System;
using System.Linq;
using Model.Accounting;
using NodaTime;
using NUnit.Framework;

namespace Tests.Mocks
{
    public static class CreateJournal
    {
        public static Journal PostedAt(LocalTime journalTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 23, journalTime.Hour, journalTime.Minute, journalTime.Second),
                new DateTime(2012,1,3), Enumerable.Empty<JournalLine>());
        }

        public static Journal Affecting(DateTime dateTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 1),
                dateTime, Enumerable.Empty<JournalLine>());
        }

        public static Journal PostedOn(DayOfWeek day)
        {
            var dayOfMonth = 6 + (int) day; //the 6th of July 2014 was a Sunday, Sunday is the 0th element of the enum.
            var journal = new Journal(Guid.NewGuid(), 
                new DateTime(2014, 7, dayOfMonth),
                new DateTime(), Enumerable.Empty<JournalLine>());

            Assert.AreEqual(day, journal.Created.DayOfWeek,
                "PostedOn should return a journal posted on the right day of the week");
            
            return journal;
        }

        public static Journal PostedTo(string accountCode1, string accountCode2)
        {
            return PostedTo(accountCode1, accountCode2, new DateTime(1999, 12, 1));
        }

        public static Journal PostedTo(string accountCode1, string accountCode2, DateTime journalDate)
        {
            return new Journal(Guid.NewGuid(), new DateTime(1999, 12, 1), journalDate,
                new[]
                {
                    new JournalLine(accountCode1, accountCode1, JournalType.Cr, 2.2m),
                    new JournalLine(accountCode2, accountCode2, JournalType.Dr, 2.2m)
                });
        }

        public static Journal ForAmount(DateTime creationDate, DateTime journalDate, int amountOfPence)
        {
            var amountOfPounds = ((decimal) amountOfPence)/100;
            return new Journal(Guid.NewGuid(), creationDate, journalDate, new []{ new JournalLine("a", "a", JournalType.Cr, amountOfPounds), new JournalLine("b", "b", JournalType.Dr, amountOfPounds)});
        }
    }
}