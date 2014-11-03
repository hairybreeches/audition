using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Audition;
using Audition.Chromium;
using Audition.Controllers;
using Audition.Native;
using Audition.Session;
using Autofac;
using CefSharp;
using Microsoft.Owin.FileSystems;
using Model.Accounting;
using Newtonsoft.Json;
using NSubstitute;
using Persistence;
using Sage50;
using Xero;

namespace SystemTests
{
    public static class AutofacConfiguration
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

        public static IContainer BuildSearchable(this ContainerBuilder builder, IEnumerable<Journal> journals)
        {
            var lifetime = builder.Build();
            lifetime.Resolve<JournalRepository>().UpdateJournals(journals);
            lifetime.Resolve<JournalSearcherFactoryStorage>().CurrentSearcherFactory = new Sage50SearcherFactory();
            return lifetime;
        }

        public static ContainerBuilder CreateDefaultContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();

            builder.Register(_ => new PhysicalFileSystem("."))
                .As<IFileSystem>();

            return builder;
        }

        public static ContainerBuilder SaveExportedFilesTo(this ContainerBuilder builder, string fileName)
        {
            var fileChooser = Substitute.For<IFileSaveChooser>();
            fileChooser.GetFileSaveLocation().Returns(Task.FromResult(fileName));
            builder.Register(_ => fileChooser).As<IFileSaveChooser>();
            return builder;
        }
    }
}