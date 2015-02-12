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
        public void TryingToAccessSessionWhenNotLoggedInGivesANotLoggedInException()
        {
            var builder = GetContainerBuilder();
            using (var container = builder.Build())
            {
                AssertSearchingGivesNotLoggedInException(container);
            }
        }        

        [Test]
        public void OnceLoggedInCanAccessSession()
        {
            var builder = GetContainerBuilder();
            using (var container = builder.Build())
            {
                Login(container);
                Assert.NotNull(container.Resolve<SearchController>());                    
            }
        }        

        [Test]
        public void CannotAccessSessionAfterLogout()
        {
            var builder = GetContainerBuilder();
            using (var container = builder.Build())
            {
                Login(container);
                container.Logout();
                AssertSearchingGivesNotLoggedInException(container);
            }
        }        

        private static ContainerBuilder GetContainerBuilder()
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder()
                .WithNoLicensing()
                .Sage50LoginReturns(new Journal(null, new DateTimeOffset(), new DateTime(), null, null, Enumerable.Empty<JournalLine>()));            
            return builder;
        }

        private static void AssertSearchingGivesNotLoggedInException(IContainer container)
        {
            var searcher = container.Resolve<SearchController>();
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(1),
                new DateRange(DateTime.MinValue, DateTime.MaxValue));

            Assert.Throws<NotLoggedInException>(
                () => searcher.AccountsSearch(new SearchRequest<UnusualAccountsParameters>(searchWindow, 1)));
        }

        private static void Login(IContainer container)
        {
            container.LoginToSage50(new Sage50LoginDetails());
        }
    }
}
