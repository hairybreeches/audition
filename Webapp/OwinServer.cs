using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Owin;

namespace Webapp
{
    public class OwinServer
    {
        private readonly IFileSystem fileSystem;
        private readonly TestServer owinTestServer;

        public OwinServer(IFileSystem fileSystem, IDependencyResolver dependencyResolver, JsonSerializerSettings jsonSettings)
        {
            this.fileSystem = fileSystem;

            owinTestServer = TestServer.Create(app => app
                .UseFileServer(GetFileOptions())
                .UseWebApi(GetApiOptions(dependencyResolver, jsonSettings)));

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

        private static HttpConfiguration GetApiOptions(IDependencyResolver dependencyResolver, JsonSerializerSettings jsonSettings)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.DependencyResolver = dependencyResolver;
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;            
            config.Formatters.JsonFormatter.SerializerSettings = jsonSettings;
            return config;
        }

        public async Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage message)
        {            
            return await owinTestServer.HttpClient.SendAsync(message);
        }               
    }
}
