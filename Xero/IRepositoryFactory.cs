using System.Threading.Tasks;
using Model.Searching;

namespace Xero
{
    public interface IRepositoryFactory
    {
        Task<JournalRepository> CreateRepository(string verificationCode);        
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}