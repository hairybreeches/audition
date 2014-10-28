namespace Xero
{
    public interface IXeroSession
    {
        void ExchangeRequestTokenForAccessToken(string verificationCode);
        void GetRequestToken();
        string GetUserAuthorizationUrl();
        IXeroJournalSource GetJournalSource();
    }
}