using System;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using CefSharp;
using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;
using NLog;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NodaTime.Text;
using NodaTime.TimeZones;

namespace Audition.Chromium
{
    public class ChromiumModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppForm>();
            builder.RegisterType<ChromiumControl>().SingleInstance();
            builder.RegisterType<OwinServer>();
            builder.RegisterType<RequestHandler>().As<IRequestHandler>();
            builder.Register(_ => new PhysicalFileSystem("ui")).As<IFileSystem>();
            builder.RegisterType<AutofacWebApiDependencyResolver>().As<IDependencyResolver>();
            builder.RegisterType<LogFactory>().SingleInstance();
            //todo: http://stackoverflow.com/questions/6623431/passing-in-the-type-of-the-declaring-class-for-nlog-using-autofac
            builder.Register(c => c.Resolve<LogFactory>().GetCurrentClassLogger());
            builder.Register(_ => JsonSettings());
        }

        private JsonSerializerSettings JsonSettings()
        {
            var settings = new JsonSerializerSettings();            
            var pattern = new NodaPatternConverter<LocalTime>(LocalTimePattern.CreateWithInvariantCulture("HH':'mm"));
            settings.Converters.Add(pattern);
            return settings;
        }
    }
}
