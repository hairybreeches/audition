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
        private readonly HttpConverter httpConverter;

        public SchemeHandler(OwinServer server, HttpConverter httpConverter)
        {
            internalDomain = Routing.InternalDomain;
            this.server = server;
            this.httpConverter = httpConverter;
        }

        public bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response,
            OnRequestCompletedHandler requestCompletedCallback)
        {            
            if (request.Url.StartsWith(internalDomain))
            {
                var httpRequestMessage = httpConverter.ToOwinHttpRequest(request);
                GetResponse(httpRequestMessage).ContinueWith(requestTask =>
                {
                    Respond(response, requestTask.Result);
                    requestCompletedCallback();                    
                });
                return true;
            }
            
            return false;
        }

        private void Respond(ISchemeHandlerResponse requestResponse, HttpResponseMessage response)
        {
            using (response)
            {
                var cefSharpResponse = httpConverter.ToCefSharpResponse(response);

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
