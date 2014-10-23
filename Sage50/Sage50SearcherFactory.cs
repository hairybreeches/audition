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

        public IJournalSearcher CreateJournalSearcher(Sage50LoginDetails loginDetails)
        {
            try
            {
                var repository = repositoryFactory.CreateJournalRepository(loginDetails);
                return new JournalSearcher(
                    new WorkingHoursSearcher(repository),
                    new YearEndSearcher(repository),
                    new UnusualAccountsSearcher(repository),
                    new RoundNumberSearcher(repository),
                    new UserSearcher(repository));
            }
            catch (OdbcException e)
            {
                var error = e.Errors[0];
                if (error.SQLState == "08001")
                {
                    throw new IncorrectLoginDetailsException("The specified folder does not appear to be a Sage 50 data directory. The data directory can be found by logging in to Sage and clicking help->about from the menu.");
                }
                if (error.SQLState == "28000")
                {
                    throw new IncorrectLoginDetailsException("Incorrect username or password");
                }

                throw;
            }
        }

        
    }
}

