using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                var response = GetResponse(request.Url, request.Method, request.Body, request.GetHeaders());
                Respond(requestResponse, response);
            }

            return false;
        }

        private static void Respond(IRequestResponse requestResponse, HttpResponseMessage response)
        {
            //TODO: Copy to separate memory stream so we can dispose of parent HttpResponseMessage
            var responseContent = response.Content.ReadAsStreamAsync().Result;

            var responseHeaders = response.Headers.Concat(response.Content.Headers)
                .ToDictionary(x => x.Key, x => x.Value.First());

            var responseMime = response.IsSuccessStatusCode
                ? response.Content.Headers.ContentType.MediaType
                : "text/html"; //CEFSharp demands a MimeType of some kind...

            requestResponse.RespondWith(responseContent, responseMime, response.ReasonPhrase, (int) response.StatusCode,
                responseHeaders);
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
            var uri = requestUrl.Replace(internalDomain, String.Empty);

            var content = CreateHttpContent(requestContent, headers);


            var method = new HttpMethod(requestMethod);

            var request = new HttpRequestMessage(method, uri)
            {
                Content = content,
            };

            foreach (var pair in headers)
            {
                request.Headers.TryAddWithoutValidation(pair.Key, pair.Value);
            }
            
            return server.ExecuteRequest(request);
            
            
        }

        private static StringContent CreateHttpContent(string requestContent, IDictionary<string, string> headers)
        {
            var content = requestContent ?? "";
            if (headers.ContainsKey("Content-Type"))
            {                
                return new StringContent(content, Encoding.UTF8, headers["Content-Type"]);
            }
            else
            {
                return new StringContent(content);
            }
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
