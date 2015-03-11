using System;
using Autofac;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Owin;
using Webapp;

namespace DevServer
{
    class Program
    {
        //todo: make this a windforms app, so we can show the native windows
        static void Main(string[] args)
        {
            using (var lifetime = ContainerBuilder())
            using(WebApp.Start(Routing.InternalDomain, GetServerAction(lifetime)))
            {
                Console.WriteLine("Running...");
                Console.ReadLine(); 
            }
        }

        private static IContainer ContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WebappModule>();
            builder.Register(_ => new PhysicalFileSystem("../../../CompiledUI/UI")).As<IFileSystem>();
            return builder.Build();
        }

        private static Action<IAppBuilder> GetServerAction(IComponentContext lifetime)
        {
            return lifetime.Resolve<OwinServerConfigurer>().Configure;
        }


    }
}
