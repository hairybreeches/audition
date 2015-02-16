using Autofac;

namespace Licensing
{
    public class LicensingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LicenceStorage>().As<ILicenceStorage>();
            builder.RegisterType<LicenceVerifier>();
        }
    }
}
