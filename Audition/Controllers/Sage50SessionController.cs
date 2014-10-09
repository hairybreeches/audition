using System.Web.Http;
using Audition.Chromium;
using Audition.Session;
using Sage50;

namespace Audition.Controllers
{
    public class Sage50SessionController : RedirectController
    {
        private readonly LoginSession session;

        public Sage50SessionController(LoginSession session)
        {
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.Sage50Login)]
        public IHttpActionResult Login(Sage50LoginDetails loginDetails)
        {            
            return RedirectToView("search.html");
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
