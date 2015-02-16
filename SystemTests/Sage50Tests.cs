﻿using System;
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
                lifetime.ImportFromSage50(new Sage50ImportDetails
                {                    
                    DataDirectory = @"Sage50SampleData",
                    Username = "Manager"
                });

                var result = lifetime.GetParsedResponseContent<SearchResponse>(requestResponse).Result;

                Assert.AreEqual("1278", result.TotalResults, "We should get all the journals back");
                Assert.AreEqual(new Transaction("8", new DateTimeOffset(2010,4,27,17,16,57, TimeSpan.FromHours(1)), new DateTime(2013,12,31), "MANAGER", "Opening Balance", new[]
            {
                new LedgerEntry("1100", "Debtors Control Account", LedgerEntryType.Cr, 0.05m), 
                new LedgerEntry("9998", "Suspense Account", LedgerEntryType.Dr, 0.05m), 
                new LedgerEntry("2200", "Sales Tax Control Account", LedgerEntryType.Dr, 0)
            }), result.Transactions[7], "A random journal should be correct");
            }


        }       
    }
}
