using System;
using System.IO;
using Audition.Chromium;
using Autofac;
using CefSharp;
using Newtonsoft.Json;
using Tests;

namespace SystemTests
{
    public static class RequestExecution
    {
        public static T GetParsedResponseContent<T>(this IContainer lifetime, MockRequest request)
        {
            var json = lifetime.GetResponseContent(request);
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                throw new Exception("Could not parse: " + json);                
            }
            
        }

        public static string GetResponseContent(this IContainer lifetime, MockRequest request)
        {
            var cefSharpResponse = lifetime.ExecuteRequest(request);            
            using (var reader = new StreamReader(cefSharpResponse.ResponseStream))
            {
                return reader.ReadToEnd();
            }
        }

        public static ISchemeHandlerResponse ExecuteRequest(this IContainer lifetime, MockRequest request)
        {
            using (var requestScope = lifetime.BeginRequestScope())
            {
                var response = new MockResponse();
                var handler = requestScope.Resolve<ISchemeHandler>();
                handler.ProcessRequestAsync(request, response, null);
                return response;
            }            
        }
    }
}