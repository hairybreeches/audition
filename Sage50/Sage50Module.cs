using Autofac;
using Sage50.Parsing;
using Sage50.Parsing.Schema;

namespace Sage50
{
    public class Sage50Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SearcherFactory>();
            builder.RegisterType<Sage50JournalSearcher>();
            builder.RegisterType<JournalSchema>();
            builder.RegisterType<JournalReader>();
            builder.RegisterType<JournalLineParser>();
        }
    }
}
