using System.Data.Odbc;
using Model;
using Model.Searching;

namespace Sage50
{
    public class Sage50SearcherFactory
    {
        private readonly Sage50RepositoryFactory repositoryFactory;

        public Sage50SearcherFactory(Sage50RepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public JournalSearcher CreateJournalSearcher(Sage50LoginDetails loginDetails)
        {
            var repository = repositoryFactory.CreateJournalRepository(loginDetails);
            return new JournalSearcher(
                new WorkingHoursSearcher(repository),
                new YearEndSearcher(repository),
                new UnusualAccountsSearcher(repository),
                new RoundNumberSearcher(repository),
                new UserSearcher(repository));
        }
    }
}

