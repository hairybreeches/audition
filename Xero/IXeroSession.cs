using DevDefined.OAuth.Storage.Basic;

namespace Xero
{
    internal interface IXeroSession
    {
        void ExchangeRequestTokenForAccessToken(string verificationCode);
        void GetRequestToken();
        string GetUserAuthorizationUrl();
    }
}