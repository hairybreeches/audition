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
            builder.RegisterType<Sage50RepositoryFactory>();
            builder.RegisterType<Sage50ConnectionFactory>();
            builder.RegisterType<Sage50DriverDetector>();
        }
    }
}
