using Autofac;
using Sage50.Parsing;
using SqlImport;

namespace Sage50
{
    public class Sage50Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterType<SageJournalSchema>();
            builder.RegisterType<JournalReader>();
            builder.RegisterType<JournalLineParser>();
            builder.RegisterType<Sage50JournalGetter>().As<ISage50JournalGetter>();
            builder.RegisterType<NominalCodeLookupFactory>().As<INominalCodeLookupFactory>();
            builder.RegisterType<Sage50ConnectionFactory>().As<ISage50ConnectionFactory>();
            builder.RegisterType<Sage50DriverDetector>();
            builder.RegisterType<OdbcRegistryReader>();
            builder.RegisterType<Sage50DataDirectoryStorage>();
        }
    }
}
