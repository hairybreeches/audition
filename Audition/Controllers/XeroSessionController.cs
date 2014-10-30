using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Audition.Chromium;
using Audition.Session;
using Model;
using Model.Persistence;
using Xero;

namespace Audition.Controllers
{
    public class XeroSessionController : RedirectController
    {
        private readonly XeroSearcherFactory searcherFactory;
        private readonly LoginSession session;
        private readonly IXeroJournalGetter xeroJournalGetter;
        private readonly JournalRepository repository;

        public XeroSessionController(XeroSearcherFactory searcherFactory, LoginSession session, IXeroJournalGetter xeroJournalGetter, JournalRepository repository)
        {
            this.searcherFactory = searcherFactory;
            this.session = session;
            this.xeroJournalGetter = xeroJournalGetter;
            this.repository = repository;
        }

        [HttpPost]
        [Route(Routing.InitialiseXeroLogin)]
        public IHttpActionResult BeginAuthenticate()
        {
            xeroJournalGetter.InitialiseAuthenticationRequest();
            return Ok();
        }
        
        [HttpPost]
        [Route(Routing.XeroLogin)]
        public async Task<IHttpActionResult> PostCompleteAuthenticationRequest(XeroVerificationCode verificationCode)
        {
            var journals = await xeroJournalGetter.CreateRepository(verificationCode.Code);
            repository.ReplaceContents(journals);
            session.Login(searcherFactory);
            return Ok();
        }

        

        [HttpGet]
        [Route(Routing.XeroLogout)]
        public IHttpActionResult Logout()
        {
            xeroJournalGetter.Logout();
            session.Logout();
            repository.Clear();
            return RedirectToView("login.html");
        }
    }
}
