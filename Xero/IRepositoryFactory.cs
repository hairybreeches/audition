using System.Threading.Tasks;
using Model.Persistence;

namespace Xero
{
    public interface IRepositoryFactory
    {
        Task<InMemoryJournalRepository> CreateRepository(string verificationCode);        
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}