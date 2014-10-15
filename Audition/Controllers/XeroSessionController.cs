using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Audition.Chromium;
using Audition.Session;
using Model;
using Model.Searching;
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
            try
            {
                repositoryFactory.CompleteAuthenticationRequest(verificationCode.Code);
            }
            catch (IncorrectLoginDetailsException e)
            {
                return InternalServerError(e);
            }            
            var repository = await repositoryFactory.CreateRepository();
            session.Login(new JournalSearcher(repository));
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
