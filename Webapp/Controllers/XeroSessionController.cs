using System.Threading.Tasks;
using System.Web.Http;
using Webapp.Session;
using Xero;

namespace Webapp.Controllers
{
    public class XeroSessionController : RedirectController
    {
        private readonly XeroSearcherFactory searcherFactory;
        private readonly LoginSession session;
        private readonly IXeroJournalGetter journalGetter;

        public XeroSessionController(XeroSearcherFactory searcherFactory, LoginSession session, IXeroJournalGetter journalGetter)
        {
            this.searcherFactory = searcherFactory;
            this.session = session;
            this.journalGetter = journalGetter;
        }

        [HttpPost]
        [Route(Routing.InitialiseXeroLogin)]
        public IHttpActionResult BeginAuthenticate()
        {
            journalGetter.InitialiseAuthenticationRequest();
            return Ok();
        }
        
        [HttpPost]
        [Route(Routing.XeroLogin)]
        public async Task<IHttpActionResult> PostCompleteAuthenticationRequest(XeroVerificationCode verificationCode)
        {
            var journals = await journalGetter.GetJournals(verificationCode.Code);
            session.Login(searcherFactory, journals);
            journalGetter.Logout();
            return Ok();
        }

        

        [HttpGet]
        [Route(Routing.XeroLogout)]
        public IHttpActionResult Logout()
        {            
            session.Logout();
            return RedirectToView("login.html");
        }
    }
}
