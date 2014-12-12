using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
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
            this.internalDomain = Routing.InternalDomain;
            this.server = server;
        }

        public bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response,
            OnRequestCompletedHandler requestCompletedCallback)
        {            
            if (request.Url.StartsWith(internalDomain))
            {
                var httpRequestMessage = HttpConversion.ToOwinHttpRequest(request);
                var httpResponse = GetResponse(httpRequestMessage);
                Respond(response, httpResponse);
                requestCompletedCallback();
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

        //todo: 302 redirects should just work!
        private HttpResponseMessage GetResponse(HttpRequestMessage httpRequest)
        {
            var response = GetImmediateResponse(httpRequest);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return
                    GetResponse(HttpConversion.ToOwinHttpRequest(response.Headers.Location.ToString(), "GET", "",
                        new Dictionary<string, string>()));
            }

            return response;
        }

        private HttpResponseMessage GetImmediateResponse(HttpRequestMessage httpRequest)
        {
            return server.ExecuteRequest(httpRequest);
        }
    }
}
