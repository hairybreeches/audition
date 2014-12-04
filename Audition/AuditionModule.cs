using System.Reflection;
using Audition.Chromium;
using Autofac;
using Autofac.Integration.WebApi;
using Excel;
using Native;
using Persistence;
using Sage50;
using Webapp.Session;
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
            builder.RegisterModule<PersistenceModule>();            
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());     
        }
    }
}