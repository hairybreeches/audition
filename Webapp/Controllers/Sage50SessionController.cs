﻿using System.Web.Http;
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

        public Sage50SessionController(Session.Session session, ISage50TransactionGetter transactionGetter, ISage50ConnectionFactory connectionFactory, Sage50DataDirectoryStorage dataDirectoryStorage)
        {
            this.session = session;
            this.transactionGetter = transactionGetter;
            this.connectionFactory = connectionFactory;
            this.dataDirectoryStorage = dataDirectoryStorage;
        }

        [HttpPost]
        [Route(Routing.Sage50Import)]
        public IHttpActionResult Import(Sage50ImportDetails importDetails)
        {
            dataDirectoryStorage.AddSage50DataLocation(importDetails.DataDirectory);
            using (var connection = connectionFactory.OpenConnection(importDetails))
            {
                var transactions = transactionGetter.GetTransactions(connection);
                session.ImportData(SearcherFactory.EverythingAvailable, transactions);
            }

            return Ok();
        }               
    }
}
