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
        public static T GetParsedResponseContent<T>(this IContainer lifetime, MockRequestResponse requestResponse)
        {
            var json = lifetime.GetResponseContent(requestResponse);
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                throw new Exception("Could not parse: " + json);                
            }
            
        }

        public static string GetResponseContent(this IContainer lifetime, MockRequestResponse requestResponse)
        {
            var cefSharpResponse = lifetime.ExecuteRequest(requestResponse);            
            using (var reader = new StreamReader(cefSharpResponse.Content))
            {
                return reader.ReadToEnd();
            }
        }

        public static CefSharpResponse ExecuteRequest(this IContainer lifetime, MockRequestResponse requestResponse)
        {
            var handler = lifetime.Resolve<IRequestHandler>();
            handler.OnBeforeResourceLoad(null, requestResponse);
            return requestResponse.Response;
        }
    }
}