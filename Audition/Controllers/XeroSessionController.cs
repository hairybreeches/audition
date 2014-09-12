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

        //todo: share these routes between js and c# using CEF
        [HttpPost]
        [Route("api/xero/login")]
        public IHttpActionResult BeginAuthenticate()
        {
            repositoryFactory.InitialiseAuthenticationRequest();
            return RedirectToView("xerocompletelogin.html");
        }
        
        [HttpPost]
        [Route("api/xero/completelogin")]
        public IHttpActionResult PostCompleteAuthenticationRequest(XeroVerificationCode verificationCode)
        {
            repositoryFactory.CompleteAuthenticationRequest(verificationCode.Code);
            return RedirectToView("search.html");
        }

        [HttpPost]
        [Route("api/xero/logout")]
        public IHttpActionResult Logout()
        {
            repositoryFactory.Logout();
            return RedirectToView("login.html");
        }
    }
}
