using System.Reflection;
using Audition.Chromium;
using Audition.Native;
using Autofac;
using Autofac.Integration.WebApi;
using Excel;
using Xero;
using Module = Autofac.Module;

namespace Audition
{
    public class AuditionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<XeroModule>();
            builder.RegisterModule<NativeModule>();
            builder.RegisterModule<ChromiumModule>();
            builder.RegisterModule<ExcelModule>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());     
        }
    }
}