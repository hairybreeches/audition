using System;
using System.Collections.Generic;
using Model.SearchWindows;
using Model.Time;
using NodaTime;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DescriptionTests
    {
        [Test]
        public void DescriptionCorrectForRoundNumberSearchWindow()
        {
            var window = new SearchWindow<EndingParameters>(new EndingParameters(3),new DateRange(new DateTime(1999,1,1), new DateTime(1999, 12, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Journals Ending in at least 3 zeroes, in the period {0} to {1}", new DateTime(1999, 1, 1).ToShortDateString(), new DateTime(1999,12,31).ToShortDateString()), window.Description);
        }
        
        [Test]
        public void DescriptionCorrectForWorkingHoursSearchWindow()
        {
            var window = new SearchWindow<WorkingHoursParameters>(new WorkingHoursParameters(DayOfWeek.Monday, DayOfWeek.Thursday, new LocalTime(9, 30),new LocalTime(18,0)), new DateRange(new DateTime(2000,4,1), new DateTime(2001, 3, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Journals posted outside Monday to Thursday, 9:30 to 18:00, in the period {0} to {1}", new DateTime(2000, 4, 1).ToShortDateString(), new DateTime(2001, 3, 31).ToShortDateString()), window.Description);
        }    
        
        [Test]
        public void DescriptionCorrectForUnusualAccountsSearchWindow()
        {
            var window = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(4), new DateRange(new DateTime(2000,4,1), new DateTime(2001, 3, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Journals posted to nominal codes with fewer than 4 entries, in the period {0} to {1}", new DateTime(2000, 4, 1).ToShortDateString(), new DateTime(2001, 3, 31).ToShortDateString()), window.Description);
        }
        
        [Test]
        public void DescriptionCorrectForUserSearchWindow()
        {
            var window = new SearchWindow<UserParameters>(new UserParameters("steve\nalf\nbetty"), new DateRange(new DateTime(2000,4,1), new DateTime(2001, 3, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Journals posted by users other than steve, alf or betty, in the period {0} to {1}", new DateTime(2000, 4, 1).ToShortDateString(), new DateTime(2001, 3, 31).ToShortDateString()), window.Description);
        } 
        
        [Test]
        public void DescriptionCorrectForYearEndWindow()
        {
            var window = new SearchWindow<YearEndParameters>(new YearEndParameters(5), new DateRange(new DateTime(2000,4,1), new DateTime(2001, 3, 31)));
            StringAssert.AreEqualIgnoringCase(String.Format("Journals posted after the year end or within 5 days before, in the period {0} to {1}", new DateTime(2000, 4, 1).ToShortDateString(), new DateTime(2001, 3, 31).ToShortDateString()), window.Description);
        }


    }
}
