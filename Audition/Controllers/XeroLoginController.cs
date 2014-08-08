using System.Web.Http;
using Xero;

namespace Audition.Controllers
{
    public class XeroLoginController : ApiController
    {
        private readonly IRepositoryFactory repositoryFactory;

        public XeroLoginController(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        [HttpPost]
        [Route("api/xero/login")]
        public void BeginAuthenticate()
        {
            repositoryFactory.InitialiseAuthenticationRequest();
        }
        
        public void PostCompleteAuthenticationRequest(string verificationCode)
        {
            repositoryFactory.CompleteAuthenticationRequest(verificationCode);
        }
    }
}
