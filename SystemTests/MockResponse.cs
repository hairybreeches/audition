using System.Collections.Specialized;
using System.IO;
using CefSharp;

namespace SystemTests
{
    public class MockResponse : ISchemeHandlerResponse
    {
        public Stream ResponseStream { get; set; }
        public string MimeType { get; set; }
        public NameValueCollection ResponseHeaders { get; set; }
        public int StatusCode { get; set; }
        public int ContentLength { get; set; }
        public string RedirectUrl { get; set; }
        public bool CloseStream { get; set; }
    }
}