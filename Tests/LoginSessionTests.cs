using System;
using Audition;
using Audition.Controllers;
using Audition.Requests;
using Audition.Session;
using Autofac;
using Model.SearchWindows;
using Model.Time;
using NSubstitute;
using NUnit.Framework;
using Tests.Mocks;
using Xero;

namespace Tests
{
    [TestFixture]
    public class LoginSessionTests
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
                using (var requestScope = container.BeginRequestScope())
                {
                    Assert.NotNull(requestScope.Resolve<SearchController>());                    
                }
            }
        }        

        [Test]
        public void CannotAccessSessionAfterLogout()
        {
            var builder = GetContainerBuilder();
            using (var container = builder.Build())
            {
                Login(container);
                container.LogoutFromXero();
                AssertSearchingGivesNotLoggedInException(container);
            }
        }        

        private static ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            builder.Register(_ => new MockXeroSession()).As<IXeroSession>();
            return builder;
        }

        private static void AssertSearchingGivesNotLoggedInException(IContainer container)
        {
            using (var requestScope = container.BeginRequestScope())
            {
                var searcher = requestScope.Resolve<SearchController>();
                var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(1),
                    new DateRange(DateTime.MinValue, DateTime.MaxValue));

                Assert.Throws<NotLoggedInException>(
                    () => searcher.AccountsSearch(new SearchRequest<UnusualAccountsParameters>(searchWindow, 1)));
            }
        }

        private static void Login(IContainer container)
        {
            container.LoginToXero(new XeroVerificationCode());
        }
    }
}
