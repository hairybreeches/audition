using System;
using System.Data.Odbc;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Searching;
using Model.SearchWindows;

namespace Sage50
{
    public class Sage50SearcherFactory
    {
        private readonly Sage50RepositoryFactory repositoryFactory;

        public Sage50SearcherFactory(Sage50RepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public IJournalSearcher CreateJournalSearcher(Sage50LoginDetails loginDetails)
        {
            var repository = repositoryFactory.CreateJournalRepository(loginDetails);

            return new JournalSearcher(
                new WorkingHoursSearcher(repository),
                new YearEndSearcher(repository),
                new UnusualAccountsSearcher(repository),
                new RoundNumberSearcher(repository),
                new NotSupportedSearcher<KeywordParameters>("Feature not yet implemented for Sage 50"),
                new NotSupportedSearcher<UserParameters>("Feature not yet implemented for Sage 50"));
        }

        
    }
}

