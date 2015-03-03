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
        public void TransactionsPersistBetweenRequests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            using (var lifetime = builder.Build())
            {
                //given some transactions saved in one request

                var saveRepository = lifetime.Resolve<ITransactionRepository>();
                saveRepository.UpdateTransactions(TransactionWithId("a single stored transaction"));
                //when we make a new request
                var loadRepository = lifetime.Resolve<ITransactionRepository>();
                var transactions = loadRepository.GetTransactions().ToList();
                //the transactions should still be there
                Assert.AreEqual(transactions.Single().Id, "a single stored transaction");
            }
        }

        [Test]
        public void TransactionsClearedOnUpdate()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            using (var lifetime = builder.Build())
            {
                //given a repository with some transactions in
                var repository = lifetime.Resolve<ITransactionRepository>();
                repository.UpdateTransactions(TransactionWithId("an old transaction").Concat(TransactionWithId("another old transaction")));

                //when we update the contents of the repository
                repository.UpdateTransactions(TransactionWithId("a new transaction"));

                var transactions = repository.GetTransactions().ToList();

                //the old contents should be blatted and only the new ones remain.
                Assert.AreEqual(transactions.Single().Id, "a new transaction");
            }
        }

        private static IEnumerable<Transaction> TransactionWithId(string id)
        {
            return new[]
            {
                new Transaction(id, DateTime.MinValue, null, null, String.Empty, String.Empty)
            };
        }
    }
}
