using System.Web.Http;
using Searching;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class SessionController : ApiController
    {
        private readonly LoginSession session;

        public SessionController(LoginSession session)
        {
            this.session = session;
        }

        [HttpGet]
        [Route(Routing.SearchCapability)]
        public SearchCapability GetSearchCapability()
        {
            return session.GetCurrentSearchCapability();
        }
    }
}
