using Autofac;

namespace Sage50
{
    public class Sage50Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SearcherFactory>();
        }
    }
}
