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

        public void PostBeginAuthenticate()
        {
            repositoryFactory.InitialiseAuthenticationRequest();
        }
        
        public void PostCompleteAuthenticationRequest(string verificationCode)
        {
            repositoryFactory.CompleteAuthenticationRequest(verificationCode);
        }
    }
}
