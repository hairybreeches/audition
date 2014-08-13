using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CefSharp;

namespace Audition.Chromium
{
    internal class RequestHandler : IRequestHandler
    {

        private readonly string internalDomain;
        private readonly OwinServer server;

        public RequestHandler(OwinServer server)
        {
            this.internalDomain = Routing.InternalDomain;
            this.server = server;
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {
            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            var request = requestResponse.Request;            
            if (request.Url.StartsWith(internalDomain))
            {
                var response = GetResponse(request.Url, request.Method, request.Body, request.GetHeaders());
                Respond(requestResponse, response);
            }

            return false;
        }

        private static void Respond(IRequestResponse requestResponse, HttpResponseMessage response)
        {
            var cefSharpResponse = HttpConversions.ToCefSharpResponse(response);


            requestResponse.RespondWith(cefSharpResponse.Content, cefSharpResponse.Mime, cefSharpResponse.ReasonPhrase, cefSharpResponse.StatusCode, 
                cefSharpResponse.Headers);
        }

        //todo: 302 redirects should just work!
        private HttpResponseMessage GetResponse(string requestUrl, string requestMethod, string requestContent, IDictionary<string, string> headers)
        {
            var response = GetImmediateResponse(requestUrl, requestMethod, requestContent, headers);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return GetResponse(response.Headers.Location.ToString(), "GET", "", new Dictionary<string, string>());
            }

            return response;
        }

        private HttpResponseMessage GetImmediateResponse(string requestUrl, string requestMethod, string requestContent, IDictionary<string, string> headers)
        {
            var request = HttpConversions.ToOwinHttpRequest(requestUrl, requestMethod, requestContent, headers);

            return server.ExecuteRequest(request);
        }        

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType,
            WebHeaderCollection headers)
        {
        }

        public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength,
            ref IDownloadHandler handler)
        {
            return false;
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme,
            ref string username, ref string password)
        {
            return false;
        }
    }
}
