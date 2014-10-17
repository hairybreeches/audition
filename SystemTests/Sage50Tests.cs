using System;
using System.Linq;
using Model.Accounting;
using NUnit.Framework;
using Sage50;

namespace SystemTests
{
    public class Sage50Tests
    {
        [Test]
        public void Sage50JournalSearchReturnsJournals()
        {
            //vagues search window ever, to return all journals in the searcher
            const string searchWindow = "{'Period':{'From':'1649-01-30', 'To':'4789-7-14'},'Parameters':{'FromDay':'Monday','ToDay':'Friday','FromTime':'00:00','ToTime':'00:00'}}";

            var requestResponse = new MockRequestResponse("POST",
                searchWindow,
                "application/json", "http://localhost:1337/api/search/hours");

            using (var lifetime = SystemFoo.CreateDefaultContainerBuilder().Build())
            {
                lifetime.LoginToSage50(new Sage50LoginDetails
                {
                    DataDirectory = @"C:\Programdata\Sage\Accounts\2015\Demodata\ACCDATA",
                    Username = "Manager"
                });

                var journalsReturned = lifetime.GetParsedResponseContent<Journal[]>(requestResponse)
                    .OrderBy(x=>int.Parse(x.Id))
                    .ToArray();

                Assert.AreEqual(1238, journalsReturned.Count(), "We should get all the journals back");
                Assert.AreEqual(new Journal("26", DateTime.Parse("27/04/2010 17:16:57"), DateTime.Parse("31/12/2013"), "MANAGER", "Unpresented Cheque", new[]
            {
                new JournalLine("1200", "Bank Current Account", JournalType.Dr, 55), 
                new JournalLine("9998", "Suspense Account", JournalType.Cr, 55), 
                new JournalLine("2200", "Sales Tax Control Account", JournalType.Dr, 0)
            }), journalsReturned[25], "A random journal should be correct");
            }


        }       
    }
}
