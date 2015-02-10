using System;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;

namespace Webapp
{
    public class OwinServerConfigurer
    {
        private readonly IFileSystem fileSystem;
        private readonly IDependencyResolver dependencyResolver;
        private readonly JsonSerializerSettings jsonSettings;

        public OwinServerConfigurer(IFileSystem fileSystem, IDependencyResolver dependencyResolver, JsonSerializerSettings jsonSettings)
        {
            this.fileSystem = fileSystem;
            this.dependencyResolver = dependencyResolver;
            this.jsonSettings = jsonSettings;
        }

        public Action<IAppBuilder> ConfigurationAction()
        {
            return app => app
                .UseFileServer(GetFileOptions())
                .UseWebApi(GetApiOptions());
        }

        private FileServerOptions GetFileOptions()
        {
            return new FileServerOptions
            {
                EnableDefaultFiles = false,
                EnableDirectoryBrowsing = true,
                FileSystem = fileSystem
            };
        }

        private HttpConfiguration GetApiOptions()
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.DependencyResolver = dependencyResolver;
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Formatters.JsonFormatter.SerializerSettings = jsonSettings;
            return config;
        }
    }
}