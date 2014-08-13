using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using CefSharp;

namespace Audition.Chromium
{
    public static class HttpConversions
    {
        private const string InternalDomain = Routing.InternalDomain;

        public static CefSharpResponse ToCefSharpResponse(HttpResponseMessage response)
        {
            //TODO: Copy to separate memory stream so we can dispose of parent HttpResponseMessage
            var responseContent = response.Content.ReadAsStreamAsync().Result;

            var responseHeaders = response.Headers.Concat(response.Content.Headers)
                .ToDictionary(x => x.Key, x => x.Value.First());

            var responseMime = response.IsSuccessStatusCode
                ? response.Content.Headers.ContentType.MediaType
                : "text/html"; //CEFSharp demands a MimeType of some kind...

            var cefSharpResponse = new CefSharpResponse(responseContent, responseMime, response.ReasonPhrase,
                (int) response.StatusCode, responseHeaders);
            return cefSharpResponse;
        }

        public static HttpRequestMessage ToOwinHttpRequest(IRequest request)
        {
            return ToOwinHttpRequest(request.Url, request.Method, request.Body, request.GetHeaders());
        }

        public static HttpRequestMessage ToOwinHttpRequest(string requestUrl, string requestMethod, string requestContent,
            IDictionary<string, string> headers)
        {
            var uri = requestUrl.Replace(InternalDomain, String.Empty);

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
            return request;
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
    }
}