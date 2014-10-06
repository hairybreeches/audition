using System.Threading.Tasks;
using XeroApi;

namespace Xero
{
    public interface IRepositoryFactory
    {
        Task<IFullRepository> CreateRepository();
        void CompleteAuthenticationRequest(string verificationCode);
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}