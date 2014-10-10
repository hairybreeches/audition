using Autofac;
using Sage50.Parsing;

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
