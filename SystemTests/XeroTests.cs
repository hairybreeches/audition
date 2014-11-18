using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Audition.Chromium;
using Autofac;
using Model.Accounting;
using Model.Responses;
using NSubstitute;
using NUnit.Framework;
using Tests;
using Tests.Mocks;
using Xero;
using XeroApi.Model;
using Journal = Model.Accounting.Journal;
using JournalLine = Model.Accounting.JournalLine;

namespace SystemTests
{
    [TestFixture]
    public class XeroTests
    {
        private readonly IXeroSession xeroSession = new MockXeroSession(
            CreateXeroJournal(new Guid("0421c274-2f50-49e4-8f61-623a4daf67ac"), new DateTime(2013, 4, 6), new DateTime(2013, 4, 6), 
                    new JournalLine("9012", "Expenses", JournalType.Cr, 23.4m),
                    new JournalLine("3001", "Cash", JournalType.Dr, 23.4m)),

            CreateXeroJournal(new Guid("c8d99cf8-6867-4767-be1e-abdf54a2a0f8"), new DateTime(2013, 4, 6), new DateTime(2013, 4, 6),                
                    new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                    new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                ),
            //this one is outside the financial period
            CreateXeroJournal(Guid.NewGuid(), new DateTime(2012, 6, 5), new DateTime(2012, 6, 5),               
                    new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                    new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                ),
            //this one is "boring"
            CreateXeroJournal(Guid.NewGuid(), new DateTime(2014, 4, 2, 12, 0, 0), new DateTime(2013, 6, 5),               
                    new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                    new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                ));

        private static XeroApi.Model.Journal CreateXeroJournal(Guid journalId, DateTime createdDateUtc, DateTime journalDate, params JournalLine[] lines)
        {
            var xeroJournal = new XeroApi.Model.Journal
            {
                JournalID = journalId,
                CreatedDateUTC = createdDateUtc,
                JournalDate = journalDate,
                JournalLines = new JournalLines()
            };
            foreach (var line in lines)
            {
                xeroJournal.JournalLines.Add(new XeroApi.Model.JournalLine()
                {
                    AccountCode = line.AccountCode,
                    AccountName = line.AccountName,
                    NetAmount = line.Amount * (line.JournalType == JournalType.Dr? 1 : -1)
                });
            }
            return xeroJournal;
        }

        private const string SearchWindow =
            "{'Period':{'From':'2013-4-5','To':'2014-4-4'},'Parameters':{'FromDay':'Monday','ToDay':'Friday','FromTime':'08:00','ToTime':'18:00'}}";
        const string SearchRequest = "{pageNumber: 1, searchWindow: " + SearchWindow + "}";
        private const string ExportRequest = @"{
            SearchWindow:" + SearchWindow + @",

            SerialisationOptions:{
                showUsername: true,
                showDescription: false
            }

        }";

        [Test]
        public void CanSaveJournalsReturnedToAFile()
        {
            var builder = CreateContainerBuilder();

            var fileName = Path.GetTempFileName();
            builder.SaveExportedFilesTo(fileName);


            var requestResponse = new MockRequestResponse("POST",
                    ExportRequest,
                    "application/json", "http://localhost:1337/api/export/hours");

            ExecuteRequest(builder, requestResponse);

            var fileContents = File.ReadAllText(fileName);

            StringAssert.AreEqualIgnoringCase(
                @"""Journals posted outside Monday to Friday, 8:00 to 18:00, in the period 05/04/2013 to 04/04/2014""
Created,Date,Username
06/04/2013 01:00:00 +01:00,06/04/2013,,Cr,9012,Expenses,23.4,Dr,3001,Cash,23.4
06/04/2013 01:00:00 +01:00,06/04/2013,,Cr,8014,Depreciation,12.4,Dr,4001,Fixed assets,12.4
", fileContents);
            
        }

        [Test]
        public void CanReturnJournalsSearchedFor()
        {
            var requestResponse = new MockRequestResponse("POST", SearchRequest, "application/json",
                   "http://localhost:1337/api/search/hours");

            var actual = GetParsedResponseContent<SearchResponse>(CreateContainerBuilder(), requestResponse);
            var expected = new SearchResponse(new[]
            {
                new Journal(Guid.Parse("0421c274-2f50-49e4-8f61-623a4daf67ac"),
                    new DateTimeOffset(2013, 4, 6, 1, 0, 0, TimeSpan.FromHours(1)), new DateTime(2013, 4, 6), new[]
                    {
                        new JournalLine("9012", "Expenses", JournalType.Cr, 23.4m),
                        new JournalLine("3001", "Cash", JournalType.Dr, 23.4m),
                    }),
                    
                    new Journal(Guid.Parse("c8d99cf8-6867-4767-be1e-abdf54a2a0f8"),
                    new DateTimeOffset(2013, 4, 6, 1, 0, 0, TimeSpan.FromHours(1)), new DateTime(2013, 4, 6), new[]
                    {
                        new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                        new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m),
                    })
            }, "2", false, false, 1);

                                
                
                Assert.AreEqual(expected, actual);
        }

        private ContainerBuilder CreateContainerBuilder()
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder();
            builder.Register(_ => xeroSession).As<IXeroSession>();            
            return builder;
        }

        private static T GetParsedResponseContent<T>(ContainerBuilder builder, MockRequestResponse requestResponse)
        {
            using (var lifetime = builder.Build())
            {
                lifetime.LoginToXero(new XeroVerificationCode());
                return lifetime.GetParsedResponseContent<T>(requestResponse);
            }
        }

        private static CefSharpResponse ExecuteRequest(ContainerBuilder builder, MockRequestResponse requestResponse)
        {
            using (var lifetime = builder.Build())
            {
                lifetime.LoginToXero(new XeroVerificationCode());
                return lifetime.ExecuteRequest(requestResponse);
            }
        }
    }
}
