using System;
using Audition;
using Audition.Controllers;
using Audition.Session;
using Autofac;
using Autofac.Core;
using Model;
using Model.SearchWindows;
using Model.Time;
using NSubstitute;
using NUnit.Framework;
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
                Logout(container);
                AssertSearchingGivesNotLoggedInException(container);
            }
        }        

        private static ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            builder.Register(_ => Substitute.For<IXeroSession>());
            return builder;
        }

        private static void AssertSearchingGivesNotLoggedInException(IComponentContext container)
        {
            var searcher = container.Resolve<SearchController>();
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(1), new DateRange(DateTime.MinValue, DateTime.MaxValue));

            Assert.Throws<NotLoggedInException>(() => searcher.AccountsSearch(searchWindow));            
        }

        private static void Login(IComponentContext container)
        {
            var loginController = container.Resolve<XeroSessionController>();
            loginController.PostCompleteAuthenticationRequest(new XeroVerificationCode());
        }

        private static void Logout(IComponentContext container)
        {
            var loginController = container.Resolve<XeroSessionController>();
            loginController.Logout();
        }
    }
}
