using Audition.Chromium;
using Autofac;
using Excel;
using Licensing;
using Native;
using Persistence;
using Sage50;
using Webapp;
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
            builder.RegisterModule<WebappModule>();
            builder.RegisterModule<LicensingModule>();
        }
    }
}