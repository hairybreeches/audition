using System;
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

        public XeroSessionController(XeroSearcherFactory searcherFactory, LoginSession session)
        {
            this.searcherFactory = searcherFactory;
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.XeroLogin)]
        public IHttpActionResult BeginAuthenticate()
        {
            searcherFactory.InitialiseAuthenticationRequest();
            return RedirectToView("xerocompletelogin.html");
        }
        
        [HttpPost]
        [Route(Routing.FinishXeroLogin)]
        public async Task<IHttpActionResult> PostCompleteAuthenticationRequest(XeroVerificationCode verificationCode)
        {
            try
            {
                session.Login(await searcherFactory.CreateXeroJournalSearcher(verificationCode.Code));
            }
            catch (IncorrectLoginDetailsException e)
            {
                return InternalServerError(e);
            }
            return Ok();
        }

        

        [HttpGet]
        [Route(Routing.XeroLogout)]
        public IHttpActionResult Logout()
        {
            searcherFactory.Logout();
            session.Logout();
            return RedirectToView("login.html");
        }
    }
}
