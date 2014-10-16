using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using CefSharp;

namespace Audition.Chromium
{
    public static class HttpConversion
    {
        private const string InternalDomain = Routing.InternalDomain;

        public static CefSharpResponse ToCefSharpResponse(HttpResponseMessage response)
        {
            var responseContent = GetResponseContent(response);

            var responseHeaders = response.Headers.Concat(response.Content.Headers)
                .ToDictionary(x => x.Key, x => x.Value.First());

            var responseMime = GetMime(response); //CEFSharp demands a MimeType of some kind...

            var cefSharpResponse = new CefSharpResponse(responseContent, responseMime, response.ReasonPhrase,
                (int) response.StatusCode, responseHeaders);
            return cefSharpResponse;
        }

        private static MemoryStream GetResponseContent(HttpResponseMessage response)
        {
            var responseContent = new MemoryStream();
            response.Content.ReadAsStreamAsync().Result.CopyTo(responseContent);
            responseContent.Position = 0;
            return responseContent;
        }

        private static string GetMime(HttpResponseMessage response)
        {
            return response != null && response.Content != null && response.Content.Headers != null &&
                   response.Content.Headers.ContentType != null &&
                   response.Content.Headers.ContentType.MediaType != null
                ? response.Content.Headers.ContentType.MediaType
                : "text/html";
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
                var contentType = new ContentType(headers["Content-Type"]);
                return new StringContent(content, Encoding.UTF8, contentType.MediaType);
            }
            else
            {
                return new StringContent(content);
            }
        }
    }
}