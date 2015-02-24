using System;
using System.Linq;
using Audition;
using Autofac;
using Model.Accounting;
using Model.Time;
using NUnit.Framework;
using Sage50;
using Searching;
using Searching.SearchWindows;
using Webapp.Controllers;
using Webapp.Requests;
using Webapp.Session;

namespace Tests
{
    [TestFixture]
    public class SessionTests
    {
        [Test]
        public void TryingToAccessSessionWhenNoDataHasBeenImportedGivesANoDataImportedException()
        {
            var builder = GetContainerBuilder();
            using (var container = builder.Build())
            {
                AssertSearchingGivesNoImportedDataException(container);
            }
        }        

        [Test]
        public void OnceDataImportedCanAccessSession()
        {
            var builder = GetContainerBuilder();
            using (var container = builder.Build())
            {
                ImportData(container);
                Assert.NotNull(container.Resolve<SearchController>());                    
            }
        }        

        [Test]
        public void CannotAccessSessionAfterClearingImport()
        {
            var builder = GetContainerBuilder();
            using (var container = builder.Build())
            {
                ImportData(container);
                container.ClearImport();
                AssertSearchingGivesNoImportedDataException(container);
            }
        }        

        private static ContainerBuilder GetContainerBuilder()
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder()
                .WithNoLicensing()
                .Sage50ImportReturns(new Transaction(null, new DateTime(), new DateTime(), null, null, Enumerable.Empty<LedgerEntry>(), String.Empty));            
            return builder;
        }

        private static void AssertSearchingGivesNoImportedDataException(IContainer container)
        {
            var searcher = container.Resolve<SearchController>();
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(1),
                new DateRange(DateTime.MinValue, DateTime.MaxValue));

            Assert.Throws<NoImportedDataException>(
                () => searcher.AccountsSearch(new SearchRequest<UnusualAccountsParameters>(searchWindow, 1)));
        }

        private static void ImportData(IContainer container)
        {
            container.ImportFromSage50(new Sage50ImportDetails());
        }
    }
}
