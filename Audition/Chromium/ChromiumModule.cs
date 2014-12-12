using Autofac;
using CefSharp;


namespace Audition.Chromium
{
    public class ChromiumModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppForm>();
            builder.RegisterType<ChromiumControl>();

            builder.RegisterType<SchemeHandler>().As<ISchemeHandler>();                       
            builder.RegisterType<SchemeHandlerFactory>().As<ISchemeHandlerFactory>();                       
        }
    }
}
