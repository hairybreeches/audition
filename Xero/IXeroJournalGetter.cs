using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Accounting;
using Model.Persistence;

namespace Xero
{
    public interface IXeroJournalGetter
    {
        Task<IEnumerable<Journal>> CreateRepository(string verificationCode);        
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}