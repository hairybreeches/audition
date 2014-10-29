using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Audition.Chromium;
using Audition.Session;
using Model;
using Xero;

namespace Audition.Controllers
{
    public class XeroSessionController : RedirectController
    {
        private readonly XeroSearcherFactory searcherFactory;
        private readonly LoginSession session;
        private readonly IRepositoryFactory repositoryFactory;

        public XeroSessionController(XeroSearcherFactory searcherFactory, LoginSession session, IRepositoryFactory repositoryFactory)
        {
            this.searcherFactory = searcherFactory;
            this.session = session;
            this.repositoryFactory = repositoryFactory;
        }

        [HttpPost]
        [Route(Routing.InitialiseXeroLogin)]
        public IHttpActionResult BeginAuthenticate()
        {
            repositoryFactory.InitialiseAuthenticationRequest();
            return Ok();
        }
        
        [HttpPost]
        [Route(Routing.XeroLogin)]
        public async Task<IHttpActionResult> PostCompleteAuthenticationRequest(XeroVerificationCode verificationCode)
        {
            var repository = await repositoryFactory.CreateRepository(verificationCode.Code);
            session.Login(searcherFactory, repository);
            return Ok();
        }

        

        [HttpGet]
        [Route(Routing.XeroLogout)]
        public IHttpActionResult Logout()
        {
            repositoryFactory.Logout();
            session.Logout();
            return RedirectToView("login.html");
        }
    }
}
