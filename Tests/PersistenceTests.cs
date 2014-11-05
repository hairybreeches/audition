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
        private static readonly DateRange Forever = new DateRange(DateTime.MinValue, DateTime.MaxValue);

        [Test]
        public void JournalsPersistBetweenRequests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            using (var lifetime = builder.Build())
            {
                //given some journals saved in one request
                using (var request = lifetime.BeginRequestScope())
                {
                    var repository = request.Resolve<JournalRepository>();
                    repository.UpdateJournals(JournalWithId("a single stored journal"));
                }
                //when we make a new request
                using (var request = lifetime.BeginRequestScope())
                {
                    var repository = request.Resolve<JournalRepository>();
                    var journals = repository.GetJournalsApplyingTo(Forever).ToList();
                    //the journals should still be there
                    Assert.AreEqual(journals.Single().Id, "a single stored journal");
                }
            }
        }

        [Test]
        public void JournalsClearedOnUpdate()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            using (var lifetime = builder.Build())
            {
                using (var request = lifetime.BeginRequestScope())
                {
                    //given a repository with some journals in
                    var repository = request.Resolve<JournalRepository>();
                    repository.UpdateJournals(JournalWithId("an old journal"));

                    //when we update the contents of the repository
                    repository.UpdateJournals(JournalWithId("a new journal"));

                    var journals = repository.GetJournalsApplyingTo(Forever).ToList();

                    //the old contents should be blatted and only the new ones remain.
                    Assert.AreEqual(journals.Single().Id, "a new journal");
                } 
            }
        }

        private static IEnumerable<Journal> JournalWithId(string id)
        {
            return new[]
            {
                new Journal(id, DateTimeOffset.MinValue, DateTime.MaxValue, null, null,
                    Enumerable.Empty<JournalLine>()),
            };
        }
    }
}
