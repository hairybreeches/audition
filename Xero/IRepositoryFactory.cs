using System.Threading.Tasks;
using Model;
using Model.Searching;
using XeroApi;

namespace Xero
{
    public interface IRepositoryFactory
    {
        Task<JournalRepository> CreateRepository();
        void CompleteAuthenticationRequest(string verificationCode);
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}