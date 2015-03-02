using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Audition.Chromium;
using CefSharp;
using NSubstitute;
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
            using (var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("I am some content", Encoding.UTF32, "random/encoding"),
                ReasonPhrase = "Yay!"
            })
            {
                response.Headers.Add("steve", "headerValue");

                //when we convert it
                var converted = new HttpConverter().ToCefSharpResponse(response);

                //then the fields are all transferred correctly
                Assert.AreEqual(200, converted.StatusCode);
                CollectionAssert.AreEquivalent(new[]
                {
                    new KeyValuePair<string, string>("steve", "headerValue"),
                    new KeyValuePair<string, string>("Content-Type", "random/encoding; charset=utf-32"),
                }, converted.Headers.ToDictionary());
                Assert.AreEqual("random/encoding", converted.Mime);
                Assert.AreEqual("I am some content", StreamToString(converted.Content, Encoding.UTF32));
                Assert.AreEqual("Yay!", converted.ReasonPhrase);
            }
        } 
        
        [Test]
        public void CanConvertNonSuccessCefsharpResponse()
        {
            //given an http response
            using (var response = new HttpResponseMessage(HttpStatusCode.RedirectMethod)
            {
                Content = new StringContent("I am some other content", Encoding.ASCII, "application/json"),
                ReasonPhrase = "Go elsewhere!"
            })
            {
                response.Headers.Add("steve", "headerValue");

                //when we convert it
                var converted = new HttpConverter().ToCefSharpResponse(response);

                //then the fields are all transferred correctly
                Assert.AreEqual(303, converted.StatusCode);
                CollectionAssert.AreEquivalent(new[]
                {
                    new KeyValuePair<string, string>("steve", "headerValue"),
                    new KeyValuePair<string, string>("Content-Type", "application/json; charset=us-ascii"),
                }, converted.Headers.ToDictionary());
                Assert.AreEqual("application/json", converted.Mime);
                Assert.AreEqual("I am some other content", StreamToString(converted.Content, Encoding.ASCII));
                Assert.AreEqual("Go elsewhere!", converted.ReasonPhrase);
            }
        }

        [Test]
        public void CanConvertFromCefSharpRequest()
        {
            var request = Substitute.For<IRequest>();
            request.Method.Returns("POST");
            request.Body.Returns("I am some content");
            var headers = new NameValueCollection();
            headers["Content-Type"] = "application/json; charset=us-ascii";
            headers["Accept"] = "text/html";
            request.Headers.Returns(headers);            


            var converted = new HttpConverter().ToOwinHttpRequest(request);

            Assert.AreEqual("I am some content", converted.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpMethod.Post, converted.Method);
            Assert.AreEqual("text/html", converted.Headers.Accept.Single().MediaType);
            Assert.AreEqual("application/json", converted.Content.Headers.ContentType.MediaType);            
        }

        private static string StreamToString(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
