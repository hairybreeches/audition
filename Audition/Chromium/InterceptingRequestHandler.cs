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
            this.internalDomain = Routing.InternalDomain;
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
                var response = GetResponse(request.Url, request.Method, request.Body);
                Respond(requestResponse, response);
            }

            return false;
        }

        private static void Respond(IRequestResponse requestResponse, HttpResponseMessage response)
        {
            //TODO: Copy to separate memory stream so we can dispose of parent HttpResponseMessage
            var responseContent = response.Content.ReadAsStreamAsync().Result;

            var responseHeaders = response.Headers.ToDictionary(x => x.Key, x => x.Value.First());

            var responseMime = response.IsSuccessStatusCode
                ? response.Content.Headers.ContentType.MediaType
                : "text/html"; //CEFSharp demands a MimeType of some kind...

            requestResponse.RespondWith(responseContent, responseMime, response.ReasonPhrase, (int) response.StatusCode,
                responseHeaders);
        }

        //todo: 302 redirects should just work!
        private HttpResponseMessage GetResponse(string requestUrl, string requestMethod, string requestContent)
        {
            var response = GetImmediateResponse(requestUrl, requestMethod, requestContent);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return GetResponse(response.Headers.Location.ToString(), "GET", "");
            }

            return response;
        }

        private HttpResponseMessage GetImmediateResponse(string requestUrl, string requestMethod, string requestContent)
        {
            var uri = requestUrl.Replace(internalDomain, String.Empty);

            var method = requestMethod.ToUpper();
            var content = CreateHttpContent(requestContent);

            switch (method)
            {
                case "GET":
                    return server.GetRequest(uri);
                case "DELETE":
                    return server.DeleteRequest(uri);
                case "PUT":
                    return server.PutRequest(uri, content);
                case "POST":
                    return server.PostRequest(uri, content);
                default:
                    throw new InvalidHttpMethod(method);
            }
            
        }

        private static StringContent CreateHttpContent(string requestContent)
        {
            return new StringContent(requestContent ?? "");
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
