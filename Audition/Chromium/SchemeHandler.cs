using System.Net.Http;
using System.Threading.Tasks;
using CefSharp;
using Webapp;

namespace Audition.Chromium
{
    internal class SchemeHandler : ISchemeHandler
    {

        private readonly string internalDomain;
        private readonly OwinServer server;

        public SchemeHandler(OwinServer server)
        {
            internalDomain = Routing.InternalDomain;
            this.server = server;
        }

        public bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response,
            OnRequestCompletedHandler requestCompletedCallback)
        {            
            if (request.Url.StartsWith(internalDomain))
            {
                var httpRequestMessage = HttpConversion.ToOwinHttpRequest(request);
                GetResponse(httpRequestMessage).ContinueWith(requestTask =>
                {
                    Respond(response, requestTask.Result);
                    requestCompletedCallback();                    
                });
                return true;
            }
            
            return false;
        }

        private static void Respond(ISchemeHandlerResponse requestResponse, HttpResponseMessage response)
        {
            using (response)
            {
                var cefSharpResponse = HttpConversion.ToCefSharpResponse(response);

                requestResponse.CloseStream = true;
                requestResponse.ResponseStream = cefSharpResponse.Content;
                requestResponse.MimeType = cefSharpResponse.Mime;
                requestResponse.StatusCode = cefSharpResponse.StatusCode;
                requestResponse.ResponseHeaders = cefSharpResponse.Headers;
            }
        }

        private async Task<HttpResponseMessage> GetResponse(HttpRequestMessage httpRequest)
        {
            return await server.ExecuteRequest(httpRequest);
        }
    }
}
