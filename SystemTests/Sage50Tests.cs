using System;
using System.Linq;
using Model;
using Model.Accounting;
using Model.Responses;
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
            const string searchRequest = "{pageNumber: 1, searchWindow: " + searchWindow + "}";

            var requestResponse = new MockRequestResponse("POST",
                searchRequest,
                "application/json", "http://localhost:1337/api/search/hours");

            using (var lifetime = AutofacConfiguration.CreateDefaultContainerBuilder().Build())
            {
                lifetime.LoginToSage50(new Sage50LoginDetails
                {
                    DataDirectory = @"C:\Programdata\Sage\Accounts\2015\Demodata\ACCDATA",
                    Username = "Manager"
                });

                var result = lifetime.GetParsedResponseContent<SearchResponse>(requestResponse);

                Assert.AreEqual(1278, result.TotalResults, "We should get all the journals back");
                Assert.AreEqual(new Journal("8", DateTime.Parse("27/04/2010 17:16:57"), DateTime.Parse("31/12/2013"), "MANAGER", "Opening Balance", new[]
            {
                new JournalLine("1100", "Debtors Control Account", JournalType.Cr, 0.05m), 
                new JournalLine("9998", "Suspense Account", JournalType.Dr, 0.05m), 
                new JournalLine("2200", "Sales Tax Control Account", JournalType.Dr, 0)
            }), result.Journals[7], "A random journal should be correct");
            }


        }       
    }
}
