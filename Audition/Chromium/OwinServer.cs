using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.Testing;
using Owin;

namespace Audition.Chromium
{
    internal class OwinServer
    {
        private readonly IFileSystem fileSystem;
        private readonly TestServer owinTestServer;

        public OwinServer(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            if (fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            owinTestServer = TestServer.Create(app => app
                .UseFileServer(GetFileOptions())
                .UseWebApi(GetApiOptions()));

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

        private static HttpConfiguration GetApiOptions()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
            return config;
        }

        public HttpResponseMessage Request(string uri)
        {
            return owinTestServer.HttpClient.GetAsync(uri).Result;
        }
    }
}