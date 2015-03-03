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
            var result = GetJournalsImported(true, 128);

            Assert.AreEqual("1278", result.TotalResults, "We should get all the transactions back");
            Assert.AreEqual(
                new Transaction("1236", new DateTime(2013, 3, 31), "MANAGER", "", "Journal Credit","",
                    new LedgerEntry("1230", "Petty Cash", LedgerEntryType.Cr, 100)
                    ), result.Transactions[7], "A random transaction should be correct");
        }      
        
        [Test]
        public void ArchivedResultsNotReturnedWhenNotRequired()
        {
            var result = GetJournalsImported(false, 1);

            Assert.AreEqual("1217", result.TotalResults, "We should get only get the unarchived transactions back");
            Assert.AreEqual(
                new Transaction("8", new DateTime(2013, 12, 31), "MANAGER", "Opening Balance", "Sales Credit Note","MAC001",
                    new LedgerEntry("1100", "Debtors Control Account", LedgerEntryType.Cr, 0.05m),
                    new LedgerEntry("9998", "Suspense Account", LedgerEntryType.Dr, 0.05m),
                    new LedgerEntry("2200", "Sales Tax Control Account", LedgerEntryType.Dr, 0)
                    ), result.Transactions[7], "A random transaction should be correct");
        }

        private static SearchResponse GetJournalsImported(bool includeArchived, int pageNumber)
        {
            //vagues search window ever, to return all transactions in the searcher
            const string searchWindow =
                "{'Period':{'From':'1649-01-30', 'To':'4789-7-14'},'Parameters':{'users':'steve\r\nBarry'}";
            var searchRequest = "{pageNumber: " + pageNumber + ", searchWindow: " + searchWindow + "}";

            var requestResponse = new MockRequest("POST",
                searchRequest,
                "application/json", "http://localhost:1337/api/search/users");

            using (var lifetime = AutofacConfiguration.CreateDefaultContainerBuilder()
                .WithNoLicensing()
                .Build())
            {
                lifetime.ImportFromSage50(new Sage50ImportDetails
                {                    
                    DataDirectory = @"Sage50SampleData",
                    Username = "Manager",
                    IncludeArchived = includeArchived
                });

                return lifetime.GetParsedResponseContent<SearchResponse>(requestResponse).Result;                
            }
        }       
    }
}
