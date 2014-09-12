using System.Web.Http;
using Audition.Chromium;
using Xero;

namespace Audition.Controllers
{
    public class XeroSessionController : RedirectController
    {
        private readonly IRepositoryFactory repositoryFactory;

        public XeroSessionController(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
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
        public IHttpActionResult PostCompleteAuthenticationRequest(XeroVerificationCode verificationCode)
        {
            repositoryFactory.CompleteAuthenticationRequest(verificationCode.Code);
            return RedirectToView("search.html");
        }

        [HttpGet]
        [Route(Routing.XeroLogout)]
        public IHttpActionResult Logout()
        {
            repositoryFactory.Logout();
            return RedirectToView("login.html");
        }
    }
}
