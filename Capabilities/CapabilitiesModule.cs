using Autofac;

namespace Capabilities
{
    public class CapabilitiesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SearchActionProvider>();
        }
    }
}
