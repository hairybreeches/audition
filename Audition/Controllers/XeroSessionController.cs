using System.Threading.Tasks;
using System.Web.Http;
using Audition.Chromium;
using Audition.Session;
using Xero;

namespace Audition.Controllers
{
    public class XeroSessionController : RedirectController
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly LoginSession session;

        public XeroSessionController(IRepositoryFactory repositoryFactory, LoginSession session)
        {
            this.repositoryFactory = repositoryFactory;
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.XeroLogin)]
        public IHttpActionResult BeginAuthenticate()
        {
            repositoryFactory.InitialiseAuthenticationRequest();
            return RedirectToView("xerocompletelogin.html");
        }
        
        [HttpPost]
        [Route(Routing.FinishXeroLogin)]
        public async Task<IHttpActionResult> PostCompleteAuthenticationRequest(XeroVerificationCode verificationCode)
        {
            repositoryFactory.CompleteAuthenticationRequest(verificationCode.Code);
            var repository = await repositoryFactory.CreateRepository();
            session.Login(new XeroJournalSearcher(repository));
            return RedirectToView("search.html");
        }

        [HttpGet]
        [Route(Routing.Logout)]
        public IHttpActionResult Logout()
        {
            repositoryFactory.Logout();
            session.Logout();
            return RedirectToView("login.html");
        }
    }
}
