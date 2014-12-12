using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Audition.Chromium;
using CefSharp;
using NSubstitute;
using Tests;

namespace SystemTests
{
    public class MockRequest : IRequest
    {
        public MockRequest(string method, string content, string contentType, string url)
        {
            
            Method =method;
            Body = content;
            Url = url;
            Headers = new Dictionary<string, string>
            {
                {"Content-Type", contentType}                
            }.ToNameValueCollection();
        }

        public string Url { get; set; }
        public string Method { get; private set; }
        public string Body { get; private set; }
        public NameValueCollection Headers { get; set; }
        public TransitionType TransitionType { get; private set; }
    }
}