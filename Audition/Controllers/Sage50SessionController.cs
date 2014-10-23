using System.Web.Http;
using Audition.Chromium;
using Audition.Session;
using Sage50;

namespace Audition.Controllers
{
    public class Sage50SessionController : RedirectController
    {
        private readonly LoginSession session;
        private readonly Sage50SearcherFactory factory;

        public Sage50SessionController(LoginSession session, Sage50SearcherFactory factory)
        {
            this.session = session;
            this.factory = factory;
        }

        [HttpPost]
        [Route(Routing.Sage50Login)]
        public IHttpActionResult Login(Sage50LoginDetails loginDetails)
        {            
            session.Login(factory.CreateJournalSearcher(loginDetails));
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
