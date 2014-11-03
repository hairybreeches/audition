using System;
using System.Collections.Generic;
using System.IO;
using Audition.Chromium;
using CefSharp;
using NSubstitute;
using Tests;

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
            throw new ShouldNotHappenInThisTestException();
        }

        public void Redirect(string url)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public void RespondWith(Stream stream, string mimeType, string statusText, int statusCode, IDictionary<string, string> responseHeaders)
        {
            Response = new CefSharpResponse(stream, mimeType, statusText, statusCode, responseHeaders);
        }

        public void RespondWith(Stream stream, string mimeType)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public CefSharpResponse Response { get; private set; }
        public IRequest Request { get; private set; }
    }
}