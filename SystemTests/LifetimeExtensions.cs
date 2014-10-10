using System;
using System.IO;
using Audition.Chromium;
using Audition.Controllers;
using Autofac;
using CefSharp;
using Newtonsoft.Json;
using Sage50;
using Xero;

namespace SystemTests
{
    public static class LifetimeExtensions
    {
        public static T GetParsedResponseContent<T>(this IComponentContext lifetime, MockRequestResponse requestResponse)
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

        public static void LoginToSage50(this IComponentContext lifetime, Sage50LoginDetails loginDetails)
        {
            var loginController = lifetime.Resolve<Sage50SessionController>();
            loginController.Login(loginDetails);
        }
    }
}