using System;
using System.Net.Http;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.Testing;
using Owin;

namespace Audition.Chromium
{
    internal class OwinBasedFileServer
    {
        private readonly TestServer owinTestServer;

        public OwinBasedFileServer(IFileSystem fileSystem)
        {
            if (fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            owinTestServer = TestServer.Create(app => InitialiseOwinServer(fileSystem, app));
        }

        private static IAppBuilder InitialiseOwinServer(IFileSystem fileSystem, IAppBuilder app)
        {
            return app.UseFileServer(new FileServerOptions
            {
                EnableDefaultFiles = false,
                EnableDirectoryBrowsing = true,
                FileSystem = fileSystem
            });
        }

        public HttpResponseMessage Request(string uri)
        {
            return owinTestServer.HttpClient.GetAsync(uri).Result;
        }
    }
}