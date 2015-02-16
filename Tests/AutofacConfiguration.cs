using System.Collections.Generic;
using System.Data.Common;
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
using Searching;
using Tests.Mocks;
using UserData;
using Webapp.Controllers;
using Webapp.Session;
using IFileSystem = Microsoft.Owin.FileSystems.IFileSystem;

namespace Tests
{
    public static class AutofacConfiguration
    {

        public static void ClearImport(this IContainer container)
        {
            var sessionController = container.Resolve<SessionController>();
            sessionController.ClearImport();
        }

        public static void ImportFromSage50(this IContainer lifetime, Sage50ImportDetails importDetails)
        {
            var sage50Controller = lifetime.Resolve<Sage50SessionController>();
            sage50Controller.Import(importDetails);
        }

        public static IContainer BuildSearchable(this ContainerBuilder builder, IEnumerable<Transaction> journals)
        {
            var lifetime = builder.Build();
            lifetime.Resolve<IJournalRepository>().UpdateJournals(journals);
            lifetime.Resolve<JournalSearcherFactoryStorage>().CurrentSearcherFactory = SearcherFactory.EverythingAvailable;
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
        
        public static ContainerBuilder Sage50ImportReturns(this ContainerBuilder builder, params Transaction[] transactions)
        {
            builder.Register(_ => Substitute.For<ISage50ConnectionFactory>());
            var journalGetter = Substitute.For<ISage50JournalGetter>();
            journalGetter.GetJournals(Arg.Any<DbConnection>()).Returns(transactions);
            builder.Register(_ => journalGetter);
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
