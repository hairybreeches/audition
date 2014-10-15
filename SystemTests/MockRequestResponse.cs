using System;
using System.Collections.Generic;
using System.IO;
using Audition.Chromium;
using CefSharp;
using NSubstitute;

namespace SystemTests
{
    public class MockRequestResponse : IRequestResponse
    {
        public MockRequestResponse(string method, string content, string contentType, string url)
        {
            Request = Substitute.For<IRequest>();
            Request.Method.Returns(method);
            Request.Body.Returns(content);
            Request.Url.Returns(url);
            Request.GetHeaders().Returns(new Dictionary<string, string>
            {
                {"Content-Type", contentType}                
            });
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Redirect(string url)
        {
            throw new NotImplementedException();
        }

        public void RespondWith(Stream stream, string mimeType, string statusText, int statusCode, IDictionary<string, string> responseHeaders)
        {
            Response = new CefSharpResponse(stream, mimeType, statusText, statusCode, responseHeaders);
        }

        public void RespondWith(Stream stream, string mimeType)
        {
            throw new NotImplementedException();
        }

        public CefSharpResponse Response { get; private set; }
        public IRequest Request { get; private set; }
    }
}