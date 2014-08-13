using System.Linq;
using System.Net.Http;

namespace Audition.Chromium
{
    public static class HttpConversions
    {
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
    }
}