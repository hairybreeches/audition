using Autofac;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using XeroApi.OAuth;

namespace Xero
{
    public class XeroModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //this needs to be a single instance because it stores state between requests: it does the token dance for OAuth login to Xero
            builder.RegisterType<XeroJournalGetter>().As<IXeroJournalGetter>().SingleInstance();
            builder.RegisterType<XeroSlurper>();
            builder.RegisterType<XeroSearcherFactory>();
            builder.RegisterType<XeroSession>().As<IXeroSession>();
            builder.RegisterType<XeroJournalSource>().As<IXeroJournalSource>();
            builder.Register(GetSession);
        }

        private static XeroApiPublicSession GetSession(IComponentContext _)
        {
            return new XeroApiPublicSession(UserAgent, ConsumerKey, ConsumerSecret,
                new InMemoryTokenRepository())
            {
                MessageLogger = new DebugMessageLogger()
            };
        }

        private const string UserAgent = "Audition";
        private const string ConsumerKey = "1PNBBUVEELJA2NIZ4DPALJ8UIAUS9H";
        private const string ConsumerSecret = "OH9UCIP6NRRTR8BOPIIPI4YYXZNGYN";
    }
}
