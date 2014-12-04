using Autofac;
using Sage50.Parsing;
using Sage50.Parsing.Schema;

namespace Sage50
{
    public class Sage50Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Sage50SearcherFactory>();
            builder.RegisterType<JournalSchema>();
            builder.RegisterType<JournalReader>();
            builder.RegisterType<JournalLineParser>();
            builder.RegisterType<Sage50JournalGetter>();
            builder.RegisterType<NominalCodeLookupFactory>().As<INominalCodeLookupFactory>();
            builder.RegisterType<Sage50ConnectionFactory>().As<ISage50ConnectionFactory>();
            builder.RegisterType<Sage50DriverDetector>();
            builder.RegisterType<OdbcOdbcRegistryReader>().As<IOdbcRegistryReader>();
        }
    }
}
