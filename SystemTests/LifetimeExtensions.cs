using System.IO;
using Audition.Chromium;
using Audition.Controllers;
using Autofac;
using CefSharp;
using Xero;

namespace SystemTests
{
    public static class LifetimeExtensions
    {
        public static string GetResponseContent(this IComponentContext lifetime, MockRequestResponse requestResponse)
        {
            var cefSharpResponse = lifetime.ExecuteRequest(requestResponse);            
            using (var reader = new StreamReader(cefSharpResponse.Content))
            {
                return reader.ReadToEnd();
            }
        }

        public static CefSharpResponse ExecuteRequest(this IComponentContext lifetime, MockRequestResponse requestResponse)
        {
            var handler = lifetime.Resolve<IRequestHandler>();
            handler.OnBeforeResourceLoad(null, requestResponse);
            return requestResponse.Response;
        }

        public static void LoginToXero(this IComponentContext lifetime, XeroVerificationCode verificationCode)
        {
            var loginController = lifetime.Resolve<XeroSessionController>();
            loginController.PostCompleteAuthenticationRequest(verificationCode).Wait();
        }
    }
}