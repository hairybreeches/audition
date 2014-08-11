using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using CefSharp;

namespace Audition.Chromium
{
    internal class InterceptingRequestHandler : IRequestHandler
    {

        private readonly string internalDomain;
        private readonly OwinServer server;

        public InterceptingRequestHandler(OwinServer server)
        {
            this.internalDomain = WebConstants.InternalDomain;
            this.server = server;
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {
            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            /*
            * Called on the IO thread before a resource is loaded. To allow the resource to load normally return false. 
            * To redirect the resource to a new url populate the |redirectUrl| value and return false. 
            * To specify data for the resource return a CefStream object in |resourceStream|, use the |response| object to set mime type, 
            * HTTP status code and optional header values, and return false. To cancel loading of the resource return true.
            * Any modifications to |request| will be observed. If the URL in |request| is changed and |redirectUrl| is also set,
            * the URL in |request| will be used.
            */
            var request = requestResponse.Request;
            if (request.Url.StartsWith(internalDomain))
            {
                HttpResponseMessage response = GetResponse(request);

                //TODO: Copy to separate memory stream so we can dispose of parent HttpResponseMessage
                var responseContent = response.Content.ReadAsStreamAsync().Result;

                var responseHeaders = response.Headers.ToDictionary(x => x.Key, x => x.Value.First());

                var responseMime = response.IsSuccessStatusCode
                    ? response.Content.Headers.ContentType.MediaType
                    : "text/html"; //CEFSharp demands a MimeType of some kind...

                requestResponse.RespondWith(responseContent, responseMime, response.ReasonPhrase, (int)response.StatusCode, responseHeaders);

            }

            return false;
        }

        private HttpResponseMessage GetResponse(IRequest request)
        {
            var requestUri = request.Url.Replace(internalDomain, String.Empty);

            var method = request.Method.ToUpper();
            var content = CreateHttpContent(request);

            switch (method)
            {
                case "GET":
                    return server.GetRequest(requestUri);
                case "DELETE":
                    return server.DeleteRequest(requestUri);
                case "PUT":
                    return server.PutRequest(requestUri, content);
                case "POST":
                    return server.PostRequest(requestUri, content);
                default:
                    throw new InvalidHttpMethod(method);
            }
            
        }

        private static StringContent CreateHttpContent(IRequest request)
        {
            return new StringContent(request.Body ?? "");
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
