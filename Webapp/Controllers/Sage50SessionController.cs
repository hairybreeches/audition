using System.Web.Http;
using Sage50;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class Sage50SessionController : RedirectController
    {
        private readonly LoginSession session;
        private readonly Sage50SearcherFactory factory;
        private readonly Sage50JournalGetter journalGetter;
        private readonly ISage50ConnectionFactory connectionFactory;
        private readonly Sage50DataDirectoryStorage dataDirectoryStorage;

        public Sage50SessionController(LoginSession session, Sage50SearcherFactory factory, Sage50JournalGetter journalGetter, ISage50ConnectionFactory connectionFactory, Sage50DataDirectoryStorage dataDirectoryStorage)
        {
            this.session = session;
            this.factory = factory;
            this.journalGetter = journalGetter;
            this.connectionFactory = connectionFactory;
            this.dataDirectoryStorage = dataDirectoryStorage;
        }

        [HttpPost]
        [Route(Routing.Sage50Login)]
        public IHttpActionResult Login(Sage50LoginDetails loginDetails)
        {
            dataDirectoryStorage.AddSage50DataLocation(loginDetails.DataDirectory);
            using (var connection = connectionFactory.OpenConnection(loginDetails))
            {
                var journals = journalGetter.GetJournals(connection);
                session.Login(factory, journals);
            }

            return Ok();
        }               

        [HttpGet]
        [Route(Routing.Sage50Logout)]
        public IHttpActionResult Logout()
        {
            session.Logout();
            return RedirectToView("login.html");
        }
    }
}
