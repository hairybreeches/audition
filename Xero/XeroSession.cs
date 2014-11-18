using XeroApi;
using XeroApi.OAuth;

namespace Xero
{
    class XeroSession : IXeroSession
    {
        private readonly XeroApiPublicSession session;

        public XeroSession(XeroApiPublicSession session)
        {
            this.session = session;
        }

        public void ExchangeRequestTokenForAccessToken(string verificationCode)
        {
            session.ExchangeRequestTokenForAccessToken(verificationCode);
        }

        public void GetRequestToken()
        {
            session.GetRequestToken();
        }

        public string GetUserAuthorizationUrl()
        {
            return session.GetUserAuthorizationUrl();
        }

        public IXeroJournalSource GetJournalSource()
        {
            return new XeroJournalSource(new Repository(session));
        }
    }
}