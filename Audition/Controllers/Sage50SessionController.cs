using System.Web.Http;
using Audition.Chromium;
using Audition.Session;
using Model.Persistence;
using Sage50;

namespace Audition.Controllers
{
    public class Sage50SessionController : RedirectController
    {
        private readonly LoginSession session;
        private readonly Sage50SearcherFactory factory;
        private readonly Sage50JournalGetter journalGetter;
        private readonly JournalRepository repository;

        public Sage50SessionController(LoginSession session, Sage50SearcherFactory factory, Sage50JournalGetter journalGetter, JournalRepository repository)
        {
            this.session = session;
            this.factory = factory;
            this.journalGetter = journalGetter;
            this.repository = repository;
        }

        [HttpPost]
        [Route(Routing.Sage50Login)]
        public IHttpActionResult Login(Sage50LoginDetails loginDetails)
        {
            var journals = journalGetter.GetJournals(loginDetails);
            repository.ReplaceContents(journals);
            session.Login(factory);
            return Ok();
        }               

        [HttpGet]
        [Route(Routing.Sage50Logout)]
        public IHttpActionResult Logout()
        {
            session.Logout();
            repository.Clear();
            return RedirectToView("login.html");
        }
    }
}
