using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using Model;
using XeroApi;
using XeroApi.Model;
using XeroApi.OAuth;

namespace Xero
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly XeroSlurper slurper;
        private XeroApiPublicSession xeroApiPublicSession;
        private const string UserAgent = "Audition";
        private const string ConsumerKey = "1PNBBUVEELJA2NIZ4DPALJ8UIAUS9H";
        private const string ConsumerSecret = "OH9UCIP6NRRTR8BOPIIPI4YYXZNGYN";


        public RepositoryFactory(XeroSlurper slurper)
        {
            this.slurper = slurper;
            CreateNewSession();
        }

        private void CreateNewSession()
        {
            xeroApiPublicSession = new XeroApiPublicSession(UserAgent, ConsumerKey, ConsumerSecret,
                new InMemoryTokenRepository())
            {
                MessageLogger = new DebugMessageLogger()
            };
        }

        public void Logout()
        {
            CreateNewSession();
        }

        public async Task<IFullRepository> CreateRepository()
        {
            var repository = new Repository(xeroApiPublicSession);
            var journals = await slurper.Slurp(repository);
            return new RepositoryWrapper(journals.ToList());
        }

        public void CompleteAuthenticationRequest(string verificationCode)
        {
            try
            {
                xeroApiPublicSession.ExchangeRequestTokenForAccessToken(verificationCode);
            }
            catch (OAuthException e)
            {
                if (e.HResult == -2146233088)
                {
                    throw new IncorrectLoginDetailsException("The authentication code provided was incorrect");
                }

                throw;
            }            
        }

        public void InitialiseAuthenticationRequest()
        {
            xeroApiPublicSession.GetRequestToken();
            LaunchAuthenticationWindow();
        }       

        private void LaunchAuthenticationWindow()
        {
            var authorisationUrl = xeroApiPublicSession.GetUserAuthorizationUrl();
            Process.Start(authorisationUrl);
        }
    }
}
