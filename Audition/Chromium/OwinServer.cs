using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Webapp;

namespace Audition.Chromium
{
    public class OwinServer
    {
        private readonly TestServer owinTestServer;

        public OwinServer(OwinServerConfigurer configurer)
        {
            owinTestServer = TestServer.Create(configurer.Configure);
        }        

        public async Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage message)
        {            
            return await owinTestServer.HttpClient.SendAsync(message);
        }        
    }
}
