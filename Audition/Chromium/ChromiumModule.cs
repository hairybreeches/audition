using System;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.FileSystems;
using NLog;

namespace Audition.Chromium
{
    class ChromiumModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppForm>();
            builder.RegisterType<ChromiumControl>();
            builder.RegisterType<OwinServer>();
            builder.Register(_ => new PhysicalFileSystem("ui")).As<IFileSystem>();
            builder.RegisterType<AutofacWebApiDependencyResolver>().As<IDependencyResolver>();
            builder.RegisterType<LogFactory>().SingleInstance();
            //todo: http://stackoverflow.com/questions/6623431/passing-in-the-type-of-the-declaring-class-for-nlog-using-autofac
            builder.Register(c => c.Resolve<LogFactory>().GetCurrentClassLogger());
        }
    }
}
