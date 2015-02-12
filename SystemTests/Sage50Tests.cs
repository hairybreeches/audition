using System;
using System.Linq;
using Model;
using Model.Accounting;
using Model.Responses;
using NUnit.Framework;
using Sage50;
using Tests;

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

            var requestResponse = new MockRequest("POST",
                searchRequest,
                "application/json", "http://localhost:1337/api/search/hours");

            using (var lifetime = AutofacConfiguration.CreateDefaultContainerBuilder()
                .WithNoLicensing()
                .Build())
            {
                lifetime.LoginToSage50(new Sage50ImportDetails
                {                    
                    DataDirectory = @"Sage50SampleData",
                    Username = "Manager"
                });

                var result = lifetime.GetParsedResponseContent<SearchResponse>(requestResponse).Result;

                Assert.AreEqual("1278", result.TotalResults, "We should get all the journals back");
                Assert.AreEqual(new Journal("8", new DateTimeOffset(2010,4,27,17,16,57, TimeSpan.FromHours(1)), new DateTime(2013,12,31), "MANAGER", "Opening Balance", new[]
            {
                new JournalLine("1100", "Debtors Control Account", JournalType.Cr, 0.05m), 
                new JournalLine("9998", "Suspense Account", JournalType.Dr, 0.05m), 
                new JournalLine("2200", "Sales Tax Control Account", JournalType.Dr, 0)
            }), result.Journals[7], "A random journal should be correct");
            }


        }       
    }
}
