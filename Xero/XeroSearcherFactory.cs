using System.Threading.Tasks;
using Model.Searching;
using Model.SearchWindows;

namespace Xero
{
    public class XeroSearcherFactory
    {
        private readonly IRepositoryFactory repositoryFactory;

        public XeroSearcherFactory(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public async Task<IJournalSearcher> CreateXeroJournalSearcher(string verificationCode)
        {
            var repository = await repositoryFactory.CreateRepository(verificationCode);

            return (IJournalSearcher)new JournalSearcher(
                new WorkingHoursSearcher(repository),
                new YearEndSearcher(repository),
                new UnusualAccountsSearcher(repository),
                new RoundNumberSearcher(repository),
                new NotSupportedSearcher<UserParameters>("Xero does not record who raises individual journals"));
        }       

        public void InitialiseAuthenticationRequest()
        {
            repositoryFactory.InitialiseAuthenticationRequest();
        }

        public void Logout()
        {
            repositoryFactory.Logout();
        }
    }
}