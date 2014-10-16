using Audition;
using Autofac;
using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;

namespace SystemTests
{
    public static class SystemFoo
    {
        public static string MungeJson(string value)
        {           
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(value));           
        }

        public static ContainerBuilder CreateDefaultContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            builder.Register(_ => new PhysicalFileSystem("."))
                .As<IFileSystem>();

            return builder;
        }
    }
}