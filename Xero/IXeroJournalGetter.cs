using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Accounting;
using Persistence;

namespace Xero
{
    public interface IXeroJournalGetter
    {
        Task<IEnumerable<Journal>> GetJournals(string verificationCode);        
        void InitialiseAuthenticationRequest();
        void Logout();
    }
}