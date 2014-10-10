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

        public static string GetResponseContent(ContainerBuilder builder, MockRequestResponse requestResponse)
        {
            var cefSharpResponse = ExecuteRequest(builder, requestResponse);

            string actual;
            using (var reader = new StreamReader(cefSharpResponse.Content))
            {
                actual = reader.ReadToEnd();
            }
            return actual;
        }

        public static CefSharpResponse ExecuteRequest(ContainerBuilder builder, MockRequestResponse requestResponse)
        {
            using (var lifetime = builder.Build())
            {
                LoginToXero(lifetime);
                return ExecuteRequest(lifetime, requestResponse);
            }
        }

        private static CefSharpResponse ExecuteRequest(IComponentContext lifetime, MockRequestResponse requestResponse)
        {
            var handler = lifetime.Resolve<IRequestHandler>();
            handler.OnBeforeResourceLoad(null, requestResponse);
            return requestResponse.Response;
        }

        private static void LoginToXero(IComponentContext lifetime)
        {
            var loginController = lifetime.Resolve<XeroSessionController>();
            loginController.PostCompleteAuthenticationRequest(new XeroVerificationCode());
        }
    }
}