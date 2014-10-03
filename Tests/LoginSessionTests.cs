using Audition;
using Audition.Controllers;
using Audition.Session;
using Autofac;
using Autofac.Core;
using Model;
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
                AssertResolvingSearchControllerGivesNotLoggedInException(container);
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
                AssertResolvingSearchControllerGivesNotLoggedInException(container);
            }
        }        

        private static ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            builder.Register(_ => Substitute.For<IRepositoryFactory>());
            return builder;
        }

        private static void AssertResolvingSearchControllerGivesNotLoggedInException(IComponentContext container)
        {
            var exception = Assert.Throws<DependencyResolutionException>(() => container.Resolve<SearchController>());
            Assert.IsInstanceOf<NotLoggedInException>(exception.InnerException);
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
