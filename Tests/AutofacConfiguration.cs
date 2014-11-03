using System.Collections.Generic;
using System.Threading.Tasks;
using Audition;
using Audition.Controllers;
using Audition.Native;
using Audition.Session;
using Autofac;
using Microsoft.Owin.FileSystems;
using Model.Accounting;
using NSubstitute;
using Persistence;
using Sage50;
using Xero;

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

        public static void LogoutFromXero(this IContainer container)
        {
            using (var requestScope = container.BeginRequestScope())
            {
                var loginController = requestScope.Resolve<XeroSessionController>();
                loginController.Logout();
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
            lifetime.Resolve<JournalRepository>().UpdateJournals(journals);
            lifetime.Resolve<JournalSearcherFactoryStorage>().CurrentSearcherFactory = new Sage50SearcherFactory();
            return lifetime;
        }

        public static ContainerBuilder CreateDefaultContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            builder.Register(_ => new PhysicalFileSystem("."))
                .As<IFileSystem>();

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