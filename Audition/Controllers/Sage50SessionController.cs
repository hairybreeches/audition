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
        private readonly Sage50RepositoryFactory repositoryFactory;

        public Sage50SessionController(LoginSession session, Sage50SearcherFactory factory, Sage50RepositoryFactory repositoryFactory)
        {
            this.session = session;
            this.factory = factory;
            this.repositoryFactory = repositoryFactory;
        }

        [HttpPost]
        [Route(Routing.Sage50Login)]
        public IHttpActionResult Login(Sage50LoginDetails loginDetails)
        {
            var repository = repositoryFactory.CreateJournalRepository(loginDetails);
            session.Login(factory, repository);
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
