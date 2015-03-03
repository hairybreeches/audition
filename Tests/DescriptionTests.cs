using System;
using System.Collections.Generic;
using Model.Time;
using NodaTime;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;

namespace Tests
{
    [TestFixture]
    public class DescriptionTests
    {
        [Test]
        public void DescriptionCorrectForRoundNumberSearchWindow()
        {
            var window = new SearchWindow<EndingParameters>(new EndingParameters(3),new DateRange(new DateTime(1999,1,1), new DateTime(1999, 12, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Transactions Ending in at least 3 zeroes, in the period {0} to {1}", new DateTime(1999, 1, 1).ToShortDateString(), new DateTime(1999,12,31).ToShortDateString()), window.Description);
        }
        
        [Test]
        public void DescriptionCorrectForUnusualNominalCodesSearchWindow()
        {
            var window = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(4), new DateRange(new DateTime(2000,4,1), new DateTime(2001, 3, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Transactions posted to nominal codes with fewer than 4 entries, in the period {0} to {1}", new DateTime(2000, 4, 1).ToShortDateString(), new DateTime(2001, 3, 31).ToShortDateString()), window.Description);
        }
        
        [Test]
        public void DescriptionCorrectForUserSearchWindow()
        {
            var window = new SearchWindow<UserParameters>(new UserParameters("steve\nalf\nbetty"), new DateRange(new DateTime(2000,4,1), new DateTime(2001, 3, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Transactions posted by users other than steve, alf or betty, in the period {0} to {1}", new DateTime(2000, 4, 1).ToShortDateString(), new DateTime(2001, 3, 31).ToShortDateString()), window.Description);
        } 
    }
}
