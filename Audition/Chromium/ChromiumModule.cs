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

            builder.RegisterType<RequestHandler>().As<IRequestHandler>();                       
        }
    }
}
