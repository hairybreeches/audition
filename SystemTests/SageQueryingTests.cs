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
            //all the journals were created in 2010 so no results
            CollectionAssert.IsEmpty(journals);
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
