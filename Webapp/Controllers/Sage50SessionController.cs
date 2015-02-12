using System.Web.Http;
using Sage50;
using Searching;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class Sage50SessionController : RedirectController
    {
        private readonly Session.Session session;
        private readonly ISage50JournalGetter journalGetter;
        private readonly ISage50ConnectionFactory connectionFactory;
        private readonly Sage50DataDirectoryStorage dataDirectoryStorage;

        public Sage50SessionController(Session.Session session, ISage50JournalGetter journalGetter, ISage50ConnectionFactory connectionFactory, Sage50DataDirectoryStorage dataDirectoryStorage)
        {
            this.session = session;
            this.journalGetter = journalGetter;
            this.connectionFactory = connectionFactory;
            this.dataDirectoryStorage = dataDirectoryStorage;
        }

        [HttpPost]
        [Route(Routing.Sage50Import)]
        public IHttpActionResult Login(Sage50LoginDetails loginDetails)
        {
            dataDirectoryStorage.AddSage50DataLocation(loginDetails.DataDirectory);
            using (var connection = connectionFactory.OpenConnection(loginDetails))
            {
                var journals = journalGetter.GetJournals(connection);
                session.Login(JournalSearcherFactory.EverythingAvailable, journals);
            }

            return Ok();
        }               
    }
}
