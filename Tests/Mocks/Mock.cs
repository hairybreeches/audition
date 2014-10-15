using System;
using System.Linq;
using System.Threading.Tasks;
using Audition;
using Audition.Controllers;
using Autofac;
using Model;
using Model.Accounting;
using Model.Searching;
using NodaTime;
using NSubstitute;
using NUnit.Framework;
using Xero;

namespace Tests.Mocks
{
    public static class Mock
    {
        public static IJournalSearcher JournalSearcher(params Journal[] journals)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            var factory = RepositoryFactory(journals);
            builder.Register(_ => factory).As<IRepositoryFactory>();
            using (var lifetime = builder.Build())
            {
                return lifetime.Resolve<XeroSearcherFactory>().CreateXeroJournalSearcher("").Result;
            }
        }

        public static Journal JournalPostedAt(LocalTime journalTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 23, journalTime.Hour, journalTime.Minute, journalTime.Second),
                new DateTime(2012,1,3), Enumerable.Empty<JournalLine>());
        }

        public static Journal JournalAffecting(DateTime dateTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 1),
                dateTime, Enumerable.Empty<JournalLine>());
        }

        public static Journal JournalPostedOn(DayOfWeek day)
        {
            var dayOfMonth = 6 + (int) day; //the 6th of July 2014 was a Sunday, Sunday is the 0th element of the enum.
            var journal = new Journal(Guid.NewGuid(), 
                new DateTime(2014, 7, dayOfMonth),
                new DateTime(), Enumerable.Empty<JournalLine>());

            Assert.AreEqual(day, journal.Created.DayOfWeek,
                "JournalPostedOn should return a journal posted on the right day of the week");
            
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

        public static Journal JournalForAmount(DateTime creationDate, DateTime journalDate, int amountOfPence)
        {
            var amountOfPounds = ((decimal) amountOfPence)/100;
            return new Journal(Guid.NewGuid(), creationDate, journalDate, new []{ new JournalLine("a", "a", JournalType.Cr, amountOfPounds), new JournalLine("b", "b", JournalType.Dr, amountOfPounds)});
        }

        public static IRepositoryFactory RepositoryFactory(params Journal[] journals)
        {
            var repository = Repository(journals);
            var factory = Substitute.For<IRepositoryFactory>();
            factory.CreateRepository().Returns(Task.FromResult(repository));
            return factory;
        }

        private static JournalRepository Repository(params Journal[] journals)
        {
            return new JournalRepository(journals);
        }
    }
}