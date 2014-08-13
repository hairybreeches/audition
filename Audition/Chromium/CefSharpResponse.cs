using System.Collections.Generic;
using System.IO;

namespace Audition.Chromium
{
    public class CefSharpResponse
    {
        public Stream Content { get; private set; }
        public string Mime { get; private set; }
        public string ReasonPhrase { get; private set; }
        public int StatusCode { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }

        public CefSharpResponse(Stream content, string mime, string reasonPhrase, int statusCode, Dictionary<string, string> headers)
        {
            Content = content;
            Mime = mime;
            ReasonPhrase = reasonPhrase;
            StatusCode = statusCode;
            Headers = headers;
        }
    }
}