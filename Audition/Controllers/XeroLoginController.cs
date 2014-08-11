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

        [HttpPost]
        [Route("api/xero/login")]
        public IHttpActionResult BeginAuthenticate()
        {
            repositoryFactory.InitialiseAuthenticationRequest();
            return RedirectToView("xerocompletelogin.html");
        }
        
        [HttpPost]
        [Route("api/xero/completelogin")]
        public IHttpActionResult PostCompleteAuthenticationRequest([FromBody]string verificationCode)
        {
            repositoryFactory.CompleteAuthenticationRequest(verificationCode);
            return RedirectToView("search.html");
        }
    }
}
