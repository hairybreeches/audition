using XeroApi;

namespace Xero
{
    public interface IRepositoryFactory
    {
        IFullRepository CreateRepository();
        void CompleteAuthenticationRequest(string verificationCode);
        void InitialiseAuthenticationRequest();
    }
}