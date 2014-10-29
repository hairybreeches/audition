using System;
using System.IO;
using System.Threading.Tasks;
using Audition.Chromium;
using Audition.Native;
using Autofac;
using Model.Accounting;
using NSubstitute;
using NUnit.Framework;
using Tests.Mocks;
using Xero;
using XeroApi.Model;
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

            var fileChooser = Substitute.For<IFileSaveChooser>();
            var fileName = Path.GetTempFileName();
            fileChooser.GetFileSaveLocation().Returns(Task.FromResult(fileName));
            builder.Register(_ => fileChooser).As<IFileSaveChooser>();


            var requestResponse = new MockRequestResponse("POST",
                    ExportRequest,
                    "application/json", "http://localhost:1337/api/export/hours");

            ExecuteRequest(builder, requestResponse);

            var fileContents = File.ReadAllText(fileName);

            StringAssert.AreEqualIgnoringCase(
                @"""Journals posted outside Monday to Friday, 8:00 to 18:00, in the period 05/04/2013 to 04/04/2014""
Created,Date,Username
06/04/2013 01:00:00 +01:00,06/04/2013 00:00:00,,Cr,9012,Expenses,23.4,Dr,3001,Cash,23.4
06/04/2013 01:00:00 +01:00,06/04/2013 00:00:00,,Cr,8014,Depreciation,12.4,Dr,4001,Fixed assets,12.4
", fileContents);
            
        }        

        [Test]
        public void CanReturnJournalsSearchedFor()
        {
            var requestResponse = new MockRequestResponse("POST", SearchRequest, "application/json",
                   "http://localhost:1337/api/search/hours");

            var actual = GetResponseContent(CreateContainerBuilder(), requestResponse);
            const string readableJson =
@"{Journals: [
    {
        'Id':'0421c274-2f50-49e4-8f61-623a4daf67ac',
        'Created':'2013-04-06T01:00:00+01:00',
        'JournalDate':'2013-04-06T00:00:00',
        'Username':'',
        'Description':'',
        'Lines':[
            {'AccountCode':'9012','AccountName':'Expenses','JournalType':'Cr','Amount':23.4},
            {'AccountCode':'3001','AccountName':'Cash','JournalType':'Dr','Amount':23.4}
        ]
    },
    {
        'Id':'c8d99cf8-6867-4767-be1e-abdf54a2a0f8',
        'Created':'2013-04-06T01:00:00+01:00',
        'JournalDate':'2013-04-06T00:00:00',
        'Username':'',
        'Description':'',
        'Lines':[
            {'AccountCode':'8014','AccountName':'Depreciation','JournalType':'Cr','Amount':12.4},
            {'AccountCode':'4001','AccountName':'Fixed assets','JournalType':'Dr','Amount':12.4}
        ]
    }
],
TotalResults: 2}";
                
                var expectedJson = SystemFoo.MungeJson(readableJson);
                
                Assert.AreEqual(expectedJson, actual);
        }

        private ContainerBuilder CreateContainerBuilder()
        {
            var builder = SystemFoo.CreateDefaultContainerBuilder();
            builder.Register(_ => xeroSession).As<IXeroSession>();            
            return builder;
        }

        private static string GetResponseContent(ContainerBuilder builder, MockRequestResponse requestResponse)
        {
            using (var lifetime = builder.Build())
            {
                lifetime.LoginToXero(new XeroVerificationCode());
                return lifetime.GetResponseContent(requestResponse);
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
