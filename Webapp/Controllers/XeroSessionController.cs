using System.Threading.Tasks;
using System.Web.Http;
using Searching;
using Webapp.Session;
using Xero;

namespace Webapp.Controllers
{
    public class XeroSessionController : RedirectController
    {
        private readonly LoginSession session;
        private readonly IXeroJournalGetter journalGetter;

        public XeroSessionController(LoginSession session, IXeroJournalGetter journalGetter)
        {
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
            session.Login(new JournalSearcherFactory(SearchField.AccountCode,
            SearchField.AccountName,
            SearchField.Amount,
            SearchField.Created,
            SearchField.JournalDate,
            SearchField.JournalType), 
            journals);
            journalGetter.Logout();
            return Ok();
        }
    }
}
