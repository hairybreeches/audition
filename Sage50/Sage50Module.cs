using Autofac;
using Sage50.Parsing;

namespace Sage50
{
    public class Sage50Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterType<SageTransactionSchema>();
            builder.RegisterType<SageJournalReader>();
            builder.RegisterType<Sage50TransactionGetter>().As<ISage50TransactionGetter>();
            builder.RegisterType<NominalCodeLookupFactory>().As<INominalCodeLookupFactory>();
            builder.RegisterType<Sage50ConnectionFactory>().As<ISage50ConnectionFactory>();
            builder.RegisterType<Sage50DriverDetector>();
            builder.RegisterType<OdbcRegistryReader>();
            builder.RegisterType<Sage50DataDirectoryStorage>();
        }
    }
}
