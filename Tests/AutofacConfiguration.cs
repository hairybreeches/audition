using System.Collections.Generic;
using System.Threading.Tasks;
using Audition;
using Autofac;
using Licensing;
using Microsoft.Owin.FileSystems;
using Model.Accounting;
using Native;
using NSubstitute;
using Persistence;
using Sage50;
using Tests.Mocks;
using UserData;
using Webapp.Controllers;
using Webapp.Session;
using Xero;
using IFileSystem = Microsoft.Owin.FileSystems.IFileSystem;

namespace Tests
{
    public static class AutofacConfiguration
    {
        public static ILifetimeScope BeginRequestScope(this IContainer container)
        {
            return container.BeginLifetimeScope("AutofacWebRequest");
        }

        public static void LoginToXero(this IContainer lifetime, XeroVerificationCode verificationCode)
        {
            using (var requestScope = lifetime.BeginRequestScope())
            {
                var loginController = requestScope.Resolve<XeroSessionController>();
                loginController.PostCompleteAuthenticationRequest(verificationCode).Wait();
            }            
        }

        public static void Logout(this IContainer container)
        {
            using (var requestScope = container.BeginRequestScope())
            {
                var sessionController = requestScope.Resolve<SessionController>();
                sessionController.Logout();
            }
        }

        public static void LoginToSage50(this IContainer lifetime, Sage50LoginDetails loginDetails)
        {
            using (var requestScope = lifetime.BeginRequestScope())
            {
                var loginController = requestScope.Resolve<Sage50SessionController>();
                loginController.Login(loginDetails);
            }
        }

        public static IContainer BuildSearchable(this ContainerBuilder builder, IEnumerable<Journal> journals)
        {
            var lifetime = builder.Build();
            using (var requestScope = lifetime.BeginRequestScope())
            {
                requestScope.Resolve<IJournalRepository>().UpdateJournals(journals);
                requestScope.Resolve<JournalSearcherFactoryStorage>().CurrentSearcherFactory = new Sage50SearcherFactory();
            }
            return lifetime;
        }

        public static ContainerBuilder CreateDefaultContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            builder.Register(_ => new PhysicalFileSystem("."))
                .As<IFileSystem>();
            builder.RegisterType<MockUserDetailsStorage>().As<IUserDetailsStorage>();

            return builder;
        }

        public static ContainerBuilder WithNoLicensing(this ContainerBuilder builder)
        {
            builder.RegisterType<PermissiveLicenceStorage>().As<ILicenceStorage>();
            return builder;
        }

        public static ContainerBuilder SaveExportedFilesTo(this ContainerBuilder builder, string fileName)
        {
            var fileChooser = Substitute.For<IFileSaveChooser>();
            fileChooser.GetFileSaveLocation().Returns(Task.FromResult(fileName));
            builder.Register(_ => fileChooser).As<IFileSaveChooser>();
            return builder;
        }
    }
}
