using System;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.Testing;
using Owin;

namespace Audition.Chromium
{
    public class OwinServer
    {
        private readonly IFileSystem fileSystem;
        private readonly TestServer owinTestServer;

        public OwinServer(IFileSystem fileSystem, IDependencyResolver dependencyResolver)
        {
            this.fileSystem = fileSystem;
            if (fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            owinTestServer = TestServer.Create(app => app
                .UseFileServer(GetFileOptions())
                .UseWebApi(GetApiOptions(dependencyResolver)));

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

        private static HttpConfiguration GetApiOptions(IDependencyResolver dependencyResolver)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.DependencyResolver = dependencyResolver;
            return config;
        }

        public HttpResponseMessage GetRequest(string uri)
        {            
            return owinTestServer.HttpClient.GetAsync(uri).Result;
        }
        
        public HttpResponseMessage PostRequest(string uri, HttpContent content)
        {
            return owinTestServer.HttpClient.PostAsync(uri, content).Result;
        }

        public HttpResponseMessage PutRequest(string uri, HttpContent content)
        {
            return owinTestServer.HttpClient.PutAsync(uri, content).Result;
        }
        
        public HttpResponseMessage DeleteRequest(string uri)
        {
            return owinTestServer.HttpClient.DeleteAsync(uri).Result;
        }
    }
}
