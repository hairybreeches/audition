using System;
using System.Linq;
using NSubstitute;
using Xero;
using XeroApi.Model;

namespace Tests.Mocks
{
    public class MockXeroSession : IXeroSession
    {
        private readonly Journal[] journals;

        public MockXeroSession(params Journal[] journals)
        {
            this.journals = journals;
        }

        public void ExchangeRequestTokenForAccessToken(string verificationCode)
        {            
        }

        public void GetRequestToken()
        {         
        }

        public string GetUserAuthorizationUrl()
        {
            return String.Empty;
        }

        public IXeroJournalSource GetJournalSource()
        {
            var mockXeroJournalSource = Substitute.For<IXeroJournalSource>();
            mockXeroJournalSource.Journals.Returns(journals.AsQueryable());
            return mockXeroJournalSource;
        }
    }
}