using System;
using System.IO;
using System.Threading.Tasks;
using Audition.Chromium;
using Autofac;
using CefSharp;
using Newtonsoft.Json;
using Tests;

namespace SystemTests
{
    public static class RequestExecution
    {
        public static T GetParsedResponseContent<T>(this IContainer lifetime, IRequest request)
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

        public static string GetResponseContent(this IContainer lifetime, IRequest request)
        {
            var cefSharpResponse = lifetime.ExecuteRequest(request);            
            using (var reader = new StreamReader(cefSharpResponse.ResponseStream))
            {
                return reader.ReadToEnd();
            }
        }

        public static ISchemeHandlerResponse ExecuteRequest(this IContainer lifetime, IRequest request)
        {
            var handler = lifetime.Resolve<IRequestHandler>();
            handler.ProcessRequestAsync(request, response, () => { });
            return requestResponse.Response;
            }            
        }
    }
