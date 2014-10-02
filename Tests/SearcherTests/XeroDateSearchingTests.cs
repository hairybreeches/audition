using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class XeroDateSearchingTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateRange FinancialPeriod = new DateRange(YearEnd.Subtract(TimeSpan.FromDays(365)), YearEnd);

        [Test]
        public void ReturnsJournalsPostedAfterYearEnd()
        {
            
            var postYearEndJournal = new Journal(Guid.NewGuid(), YearEnd.AddDays(1), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<JournalLine>() );
            var searcher = Mock.JournalSearcher(postYearEndJournal);

            var searchParameters = new SearchWindow<YearEndParameters>(new YearEndParameters(2),
                FinancialPeriod);

            var result = searcher.FindJournalsWithin(searchParameters);
            CollectionAssert.AreEquivalent(new []{postYearEndJournal}, result);
        }
    }
}
