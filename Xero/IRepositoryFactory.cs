using System.Threading.Tasks;
using Persistence;

namespace Xero
{
    public interface IRepositoryFactory
    {
        Task<JournalRepository> CreateRepository(string verificationCode);        
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}