using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audition;
using Autofac;
using CefSharp;
using Model;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.Mocks;
using Xero;

namespace SystemTests
{
    [TestFixture]
    public class SystemTests
    {
        [Test]
        public void CanSaveJournalsReturnedToAFile()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            builder.Register(
                _ =>
                    MockXeroRepositoryFactory.Create(
                        new Journal(Guid.NewGuid(), new DateTime(2013, 4, 6), new DateTime(2013, 4, 6),
                            new List<JournalLine>
                            {
                                new JournalLine("9012", "Expenses", JournalType.Cr, 23.4m),
                                new JournalLine("3001", "Cash", JournalType.Dr, 23.4m)
                            }),
                        new Journal(Guid.NewGuid(), new DateTime(2013, 4, 6), new DateTime(2013, 4, 6),
                            new List<JournalLine>
                            {
                                new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                                new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                            }),
                    //this one is outside the financial period
                        new Journal(Guid.NewGuid(), new DateTime(2012, 6, 5), new DateTime(2012, 6, 5),
                            new List<JournalLine>
                            {
                                new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                                new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                            }),
                    //this one is "boring"
                            new Journal(Guid.NewGuid(), new DateTime(2014, 4, 2, 12, 0, 0), new DateTime(2013, 6, 5),
                            new List<JournalLine>
                            {
                                new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                                new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                            }))).As<IRepositoryFactory>();

            builder.Register(_ => new Microsoft.Owin.FileSystems.PhysicalFileSystem("."))
                .As<Microsoft.Owin.FileSystems.IFileSystem>();

            using (var lifetime = builder.Build())
            {
                var fileName = Path.GetTempFileName();
                var handler = lifetime.Resolve<IRequestHandler>();

                var requestResponse = new MockRequestResponse("POST", "{'searchWindow':{'Period':{'From':'2013-4-5','To':'2014-4-4'},'Outside':{'FromDay':'Monday','ToDay':'Friday','FromTime':'08:00','ToTime':'18:00'}},'fileName':" + JsonConvert.SerializeObject(fileName) + "}", "application/json", "http://localhost:1337/api/search/export");
                handler.OnBeforeResourceLoad(null, requestResponse);
                var fileContents = File.ReadAllText(fileName);
                StringAssert.AreEqualIgnoringCase(
@"Created,Date
06/04/2013 00:00:00,06/04/2013 00:00:00,Cr,9012,Expenses,23.4,Dr,3001,Cash,23.4
06/04/2013 00:00:00,06/04/2013 00:00:00,Cr,8014,Depreciation,12.4,Dr,4001,Fixed assets,12.4
", fileContents);
            }


        }
    }
}
