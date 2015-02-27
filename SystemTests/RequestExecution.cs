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
        public async static Task<T> GetParsedResponseContent<T>(this IContainer lifetime, IRequest request)
        {
            var json = await lifetime.GetResponseContent(request);
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                throw new Exception("Could not parse: " + json);                
            }
            
        }

        public static async Task<string> GetResponseContent(this IContainer lifetime, IRequest request)
        {
            var cefSharpResponse = await lifetime.ExecuteRequest(request);            
            using (var reader = new StreamReader(cefSharpResponse.ResponseStream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Task<ISchemeHandlerResponse> ExecuteRequest(this IContainer lifetime, IRequest request)
        {
            var t = new TaskCompletionSource<ISchemeHandlerResponse>();
            var response = new MockResponse();
            var handler = lifetime.Resolve<ISchemeHandler>();
            
            handler.ProcessRequestAsync(request, response, () => t.TrySetResult(response));
            return t.Task;
            
            }            
        }
    }
