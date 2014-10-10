using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Accounting;
using Model.SearchWindows;
using NUnit.Framework;
using Sage50;

namespace SystemTests
{
    public class Sage50Tests
    {
        [Test]
        public void Sage50JournalSearchReturnsJournals()
        {
            var requestResponse = new MockRequestResponse("POST",
                "",
                "application/json", "http://localhost:1337/api/search/hours");

            using (var lifetime = SystemFoo.CreateDefaultContainerBuilder().Build())
            {
                lifetime.LoginToSage50(new Sage50LoginDetails
                {
                    DataDirectory = @"C:\Programdata\Sage\Accounts\2015\Demodata\ACCDATA",
                    Username = "Manager"
                });

                var journalsReturned = lifetime.GetParsedResponseContent<Journal[]>(requestResponse);

                Assert.AreEqual(1234, journalsReturned.Count(), "We should get all the journals back");
                Assert.AreEqual(new Journal("26", DateTime.Parse("27/04/2010 17:16:57"), DateTime.Parse("31/12/2013"), "MANAGER", "Unpresented Cheque", new[]
            {
                new JournalLine("1200", "1200", JournalType.Dr, 55), 
                new JournalLine("9998", "9998", JournalType.Cr, 55), 
                new JournalLine("2200", "2200", JournalType.Dr, 0)
            }), journalsReturned[25], "A random journal should be correct");
            }


        }       
    }
}
