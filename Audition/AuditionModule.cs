using System.Reflection;
using Audition.Chromium;
using Autofac;
using Autofac.Integration.WebApi;
using Xero;
using Module = Autofac.Module;

namespace Audition
{
    internal class AuditionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<XeroModule>();
            builder.RegisterModule<ChromiumModule>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());     
        }
    }
}