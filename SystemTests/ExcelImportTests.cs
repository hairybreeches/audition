using System;
using System.CodeDom;
using System.Linq;
using Autofac;
using ExcelImport;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Tests;
using Webapp.Controllers;
using Webapp.Requests;

namespace SystemTests
{
    [TestFixture]
    public class ExcelImportTests
    {
        [Test]
        public void CanImportExcelData()
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder();
            using (var scope = builder.Build())
            {
                var controller = scope.Resolve<ExcelSessionController>();
                controller.ExcelLogin(new ExcelImportMapping
                {
                    SheetData = new HeaderRowData
                    {
                        Filename = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx",
                        Sheet = 0,
                        UseHeaderRow = true,
                        SkipRows = 2
                    },

                    Lookups = new FieldLookups
                    {
                        AccountCode = 3,
                        AccountName = -1,
                        Amount = 9,
                        Created = -1,
                        Description = 5,
                        JournalDate = 6,
                        Username = 19
                    }
                });

                var searchController = scope.Resolve<SearchController>();
                var results =
                    searchController.EndingSearch(
                        new SearchRequest<EndingParameters>(
                            new SearchWindow<EndingParameters>(new EndingParameters(0),
                                new DateRange(DateTime.MinValue, DateTime.MaxValue)), 7));

                Assert.AreEqual(new Journal("62", default(DateTime), new DateTime(2013, 1, 30), "MANAGER", "Rent Prepayment", 
                    new[]
                    {
                        new JournalLine("7100", null, JournalType.Dr, 450)
                    }), results.Journals[2], "A random journal should be correct");

                Assert.AreEqual(1234, results.TotalResults);
            }
        }
    }
}
