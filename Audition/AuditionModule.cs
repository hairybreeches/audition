using Audition.Chromium;
using Autofac;
using Webapp;

namespace Audition
{
    public class AuditionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<WebappModule>();            
            builder.RegisterModule<ChromiumModule>();
        }
    }
}