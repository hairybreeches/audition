using System.Reflection;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NodaTime.Text;
using Module = Autofac.Module;

namespace Webapp
{
    public class WebappModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OwinServer>();
            builder.Register(_ => new PhysicalFileSystem("ui")).As<IFileSystem>();
            builder.RegisterType<AutofacWebApiDependencyResolver>().As<IDependencyResolver>();
            builder.Register(_ => JsonSettings());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());    
        }

        private static JsonSerializerSettings JsonSettings()
        {
            var settings = new JsonSerializerSettings();
            var pattern = new NodaPatternConverter<LocalTime>(LocalTimePattern.CreateWithInvariantCulture("HH':'mm"));
            settings.Converters.Add(pattern);
            return settings;
        }
    }
}
