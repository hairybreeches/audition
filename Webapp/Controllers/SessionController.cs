using System.Web.Http;
using Searching;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class SessionController : RedirectController
    {
        private readonly Session.Session session;

        public SessionController(Session.Session session)
        {
            this.session = session;
        }

        [HttpGet]
        [Route(Routing.SearchCapability)]
        public SearchCapability GetSearchCapability()
        {
            return session.GetCurrentSearchCapability();
        }

        [HttpGet]
        [Route(Routing.Logout)]
        public IHttpActionResult Logout()
        {
            session.Logout();
            return RedirectToView("import.html");
        }
    }
}
