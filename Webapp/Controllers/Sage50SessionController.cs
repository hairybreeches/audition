using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Capabilities;
using Sage50;
using Searching;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class Sage50SessionController : RedirectController
    {
        private readonly Session.Session session;
        private readonly ISage50TransactionGetter transactionGetter;
        private readonly ISage50ConnectionFactory connectionFactory;
        private readonly Sage50DataDirectoryStorage dataDirectoryStorage;
        private readonly DisplayFieldProvider displayFieldProvider;

        public Sage50SessionController(Session.Session session, ISage50TransactionGetter transactionGetter, ISage50ConnectionFactory connectionFactory, Sage50DataDirectoryStorage dataDirectoryStorage, DisplayFieldProvider displayFieldProvider)
        {
            this.session = session;
            this.transactionGetter = transactionGetter;
            this.connectionFactory = connectionFactory;
            this.dataDirectoryStorage = dataDirectoryStorage;
            this.displayFieldProvider = displayFieldProvider;
        }

        [HttpPost]
        [Route(Routing.Sage50Import)]
        public IHttpActionResult Import(Sage50ImportDetails importDetails)
        {
            dataDirectoryStorage.AddSage50DataLocation(importDetails.DataDirectory);
            using (var connection = connectionFactory.OpenConnection(importDetails))
            {
                var transactions = transactionGetter.GetTransactions(connection);
                session.ImportData(new SearcherFactory(new Dictionary<SearchActionName, string>(), displayFieldProvider.GetAll.ToArray()), transactions);
            }

            return Ok();
        }               
    }
}
