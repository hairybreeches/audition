using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Audition;
using Autofac;
using Capabilities;
using Licensing;
using Microsoft.Owin.FileSystems;
using Model;
using Model.Accounting;
using Native;
using Native.Dialogs;
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

        public static IContainer BuildSearchable(this ContainerBuilder builder, IEnumerable<Transaction> transactions)
        {
            var lifetime = builder.Build();
            lifetime.Resolve<ITransactionRepository>().UpdateTransactions(transactions);
            lifetime.Resolve<SearcherFactoryStorage>().CurrentSearcherFactory = new SearcherFactory(Enumerable.Empty<SearchAction>(), new DisplayFieldProvider().AllFields.ToArray());
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
            var transactionGetter = Substitute.For<ISage50TransactionGetter>();
            transactionGetter.GetTransactions(Arg.Any<DbConnection>()).Returns(transactions);
            builder.Register(_ => transactionGetter);
            return builder;
        }

        public static ContainerBuilder SaveExportedFilesTo(this ContainerBuilder builder, string fileName)
        {
            var fileChooser = Substitute.For<IFileSaveChooser>();
            fileChooser.GetFileSaveLocation().Returns(Task.FromResult(ExportResult.Success(fileName)));
            builder.Register(_ => fileChooser).As<IFileSaveChooser>();
            return builder;
        }
    }
}
