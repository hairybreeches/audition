using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevDefined.OAuth.Framework;
using Model;
using Model.Accounting;
using Persistence;

namespace Xero
{
    internal class XeroJournalGetter : IXeroJournalGetter
    {
        private readonly XeroSlurper slurper;
        private IXeroSession xeroApiPublicSession;
        private readonly Func<IXeroSession> sessionFactory;



        public XeroJournalGetter(XeroSlurper slurper, Func<IXeroSession> sessionFactory)
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

        public async Task<IEnumerable<Journal>> GetJournals(string verificationCode)
        {
            CompleteAuthenticationRequest(verificationCode);
            var repository = xeroApiPublicSession.GetJournalSource();
            return await slurper.Slurp(repository);
        }

        private void CompleteAuthenticationRequest(string verificationCode)
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
            catch (ApplicationException e)
            {
                if (e.HResult == -2146232832)
                {
                    throw new IncorrectLoginDetailsException("Login to Xero and obtain an authentication code by pressing \"Get Code\"");
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
