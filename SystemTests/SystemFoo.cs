using System.IO;
using Audition;
using Audition.Chromium;
using Audition.Controllers;
using Autofac;
using CefSharp;
using Newtonsoft.Json;
using Xero;

namespace SystemTests
{
    public static class SystemFoo
    {
        public static string MungeJson(string value)
        {           
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(value));           
        }

        public static ContainerBuilder CreateDefaultContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            return builder;
        }

        public static string GetResponseContent(IComponentContext lifetime, MockRequestResponse requestResponse)
        {
            var cefSharpResponse = ExecuteRequest(lifetime, requestResponse);            
            using (var reader = new StreamReader(cefSharpResponse.Content))
            {
                return reader.ReadToEnd();
            }
        }

        public static CefSharpResponse ExecuteRequest(IComponentContext lifetime, MockRequestResponse requestResponse)
        {
            var handler = lifetime.Resolve<IRequestHandler>();
            handler.OnBeforeResourceLoad(null, requestResponse);
            return requestResponse.Response;
        }

        public static void LoginToXero(IComponentContext lifetime)
        {
            var loginController = lifetime.Resolve<XeroSessionController>();
            loginController.PostCompleteAuthenticationRequest(new XeroVerificationCode());
        }
    }
}