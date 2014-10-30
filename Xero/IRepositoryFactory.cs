using System.Threading.Tasks;
using Model.Persistence;

namespace Xero
{
    public interface IRepositoryFactory
    {
        Task<JournalRepository> CreateRepository(string verificationCode);        
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}