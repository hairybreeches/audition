using System;
using System.Diagnostics;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using XeroApi;
using XeroApi.OAuth;

namespace Xero
{
    internal class RepositoryFactory : IRepositoryFactory
    {
        private const string UserAgent = "Audition";
        private const string ConsumerKey = "1PNBBUVEELJA2NIZ4DPALJ8UIAUS9H";
        private const string ConsumerSecret = "OH9UCIP6NRRTR8BOPIIPI4YYXZNGYN";

        public Repository CreateRepository()
        {
            var consumerSession = new XeroApiPublicSession(UserAgent, ConsumerKey, ConsumerSecret,
                new InMemoryTokenRepository())
            {
                MessageLogger = new DebugMessageLogger()
            };

            consumerSession.GetRequestToken();

            LaunchAuthenticationWindow(consumerSession);

            var verificationCode = GetInputCode();

            consumerSession.ExchangeRequestTokenForAccessToken(verificationCode);

            // Wrap the authenticated consumerSession in the repository...
            return new Repository(consumerSession);
        }

        private static string GetInputCode()
        {
            Console.WriteLine("Please input the code you were given in Xero:");
            var verificationCode = Console.ReadLine();
            return verificationCode;
        }

        private static void LaunchAuthenticationWindow(XeroApiPublicSession consumerSession)
        {
            var authorisationUrl = consumerSession.GetUserAuthorizationUrl();
            Process.Start(authorisationUrl);
        }
    }
}
