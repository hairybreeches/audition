using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Audition.Chromium;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HttpConversionTests
    {
        [Test]
        public void CanConvertToCefsharpResponse()
        {
            //given an http response
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("I am some content", Encoding.UTF32, "random/encoding"),
                ReasonPhrase = "Yay!"                
            };
            response.Headers.Add("steve", "headerValue");

            //when we convert it
            var converted = HttpConversion.ToCefSharpResponse(response);

            //then the fields are all transferred correctly
            Assert.AreEqual(200, converted.StatusCode);
            CollectionAssert.AreEquivalent(new[]
            {
                new KeyValuePair<string, string>("steve", "headerValue"), 
                new KeyValuePair<string, string>("Content-Type", "random/encoding; charset=utf-32"), 
            },converted.Headers);
            Assert.AreEqual("random/encoding", converted.Mime);
            Assert.AreEqual("I am some content", StreamToString(converted.Content, Encoding.UTF32));
            Assert.AreEqual("Yay!", converted.ReasonPhrase);
        } 
        
        [Test]
        public void CanConvertNonSuccessCefsharpResponse()
        {
            //given an http response
            var response = new HttpResponseMessage(HttpStatusCode.RedirectMethod)
            {
                Content = new StringContent("I am some other content", Encoding.ASCII, "application/json"),
                ReasonPhrase = "Go elsewhere!"                
            };
            response.Headers.Add("steve", "headerValue");

            //when we convert it
            var converted = HttpConversion.ToCefSharpResponse(response);

            //then the fields are all transferred correctly
            Assert.AreEqual(303, converted.StatusCode);
            CollectionAssert.AreEquivalent(new[]
            {
                new KeyValuePair<string, string>("steve", "headerValue"), 
                new KeyValuePair<string, string>("Content-Type", "application/json; charset=us-ascii"), 
            },converted.Headers);
            Assert.AreEqual("random/encoding", converted.Mime);
            Assert.AreEqual("I am some other content", StreamToString(converted.Content, Encoding.ASCII));
            Assert.AreEqual("Go elsewhere!", converted.ReasonPhrase);
        }

        private string StreamToString(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
