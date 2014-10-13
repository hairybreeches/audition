using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Sage50;
using Sage50.Parsing;
using Sage50.Parsing.Schema;

namespace SystemTests
{
    public class SageQueryingTests
    {
        [Test]
        public void CanSearchByDate()
        {
            var journals =
                GetSearcher()
                    .FindJournalsWithin(new SearchWindow<YearEndParameters>(new YearEndParameters(3),
                        new DateRange(new DateTime(2014, 3, 1), new DateTime(2014, 3, 31))));
            CollectionAssert.AreEqual(new[]
            {
                new Journal("1237", new DateTimeOffset(2014,10,13,15,03,11, new TimeSpan(1,0,0)),new DateTime(2014,3,31), "MANAGER", "", new []
                {
                    new JournalLine("0010", "0010", JournalType.Dr, 100)
                }),
                new Journal("1238", new DateTimeOffset(2014,10,13,15,03,11, new TimeSpan(1,0,0)),new DateTime(2014,3,31), "MANAGER", "", new []
                {
                    new JournalLine("1240", "1240", JournalType.Cr, 100)
                }),
            }, journals);
        }


        IJournalSearcher GetSearcher()
        {
            return
                new SearcherFactory(
                    factory =>
                        new Sage50JournalSearcher(factory, new JournalSchema(),
                            new JournalReader(new JournalLineParser(new JournalSchema())))).CreateJournalSearcher(new Sage50LoginDetails
                            {
                                DataDirectory = @"C:\ProgramData\Sage\Accounts\2015\Demodata\ACCDATA",
                                Username = "Manager"
                            });
        }
    }
}
