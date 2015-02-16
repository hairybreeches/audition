using System;
using System.Collections.Generic;
using System.Linq;
using Audition;
using Autofac;
using Model.Accounting;
using Model.Time;
using NUnit.Framework;
using Persistence;

namespace Tests
{
    [TestFixture]
    public class PersistenceTests
    {
        [Test]
        public void JournalsPersistBetweenRequests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            using (var lifetime = builder.Build())
            {
                //given some journals saved in one request

                var saveRepository = lifetime.Resolve<IJournalRepository>();
                saveRepository.UpdateJournals(JournalWithId("a single stored journal"));
                //when we make a new request
                var loadRepository = lifetime.Resolve<IJournalRepository>();
                var journals = loadRepository.GetJournals().ToList();
                //the journals should still be there
                Assert.AreEqual(journals.Single().Id, "a single stored journal");
            }
        }

        [Test]
        public void JournalsClearedOnUpdate()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            using (var lifetime = builder.Build())
            {
                //given a repository with some journals in
                var repository = lifetime.Resolve<IJournalRepository>();
                repository.UpdateJournals(JournalWithId("an old journal").Concat(JournalWithId("another old journal")));

                //when we update the contents of the repository
                repository.UpdateJournals(JournalWithId("a new journal"));

                var journals = repository.GetJournals().ToList();

                //the old contents should be blatted and only the new ones remain.
                Assert.AreEqual(journals.Single().Id, "a new journal");
            }
        }

        private static IEnumerable<Transaction> JournalWithId(string id)
        {
            return new[]
            {
                new Transaction(id, DateTimeOffset.MinValue, DateTime.MaxValue, null, null,
                    Enumerable.Empty<JournalLine>()),
            };
        }
    }
}
