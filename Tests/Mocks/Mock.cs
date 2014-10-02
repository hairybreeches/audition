using Model;
using Model.Accounting;
using Xero;

namespace Tests.Mocks
{
    public static class Mock
    {
        public static IJournalSearcher JournalSearcher(params Journal[] journals)
        {
            var factory = MockXeroRepositoryFactory.Create(journals);
            return new XeroJournalSearcher(factory);
        }
    }
}