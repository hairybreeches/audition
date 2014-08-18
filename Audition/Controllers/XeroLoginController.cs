using System.Web.Http;
using System.Web.Http.Results;
using Xero;

namespace Audition.Controllers
{
    public class XeroLoginController : RedirectController
    {
        private readonly IRepositoryFactory repositoryFactory;

        public XeroLoginController(IRepositoryFactory repositoryFactory)
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
    }
}
