using System.Reflection;
using Audition.Chromium;
using Audition.Native;
using Audition.Session;
using Autofac;
using Autofac.Integration.WebApi;
using Excel;
using Sage50;
using Xero;
using Module = Autofac.Module;

namespace Audition
{
    public class AuditionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<XeroModule>();
            builder.RegisterModule<Sage50Module>();
            builder.RegisterModule<NativeModule>();
            builder.RegisterModule<ChromiumModule>();
            builder.RegisterModule<ExcelModule>();            
            builder.RegisterModule<SessionModule>();            
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());     
        }
    }
}