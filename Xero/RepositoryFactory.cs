using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using Model;
using Model.Searching;
using XeroApi.OAuth;

namespace Xero
{
    internal class RepositoryFactory : IRepositoryFactory
    {
        private readonly XeroSlurper slurper;
        private IXeroSession xeroApiPublicSession;
        private readonly Func<IXeroSession> sessionFactory;



        public RepositoryFactory(XeroSlurper slurper, Func<IXeroSession> sessionFactory)
        {
            this.slurper = slurper;
            this.sessionFactory = sessionFactory;
            CreateNewSession();
        }

        private void CreateNewSession()
        {
            xeroApiPublicSession = sessionFactory();
        }

        public void Logout()
        {
            CreateNewSession();
        }

        public async Task<JournalRepository> CreateRepository()
        {
            var repository = xeroApiPublicSession.GetJournalSource();
            var journals = await slurper.Slurp(repository);
            return new JournalRepository(journals.ToList());
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
