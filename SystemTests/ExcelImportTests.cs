using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ExcelImport;
using Model.Accounting;
using Model.Responses;
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
            var results = GetAllJournalsFromSearch(new ExcelImportMapping
            {
                SheetData = new HeaderRowData
                {
                    Filename = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx",
                    Sheet = 0,
                    UseHeaderRow = true,
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
            }, 7);

            Assert.AreEqual(new Journal("63", default(DateTime), new DateTime(2013, 1, 30), "MANAGER", "Rent Prepayment",
                    new[]
                    {
                        new JournalLine("7100", null, JournalType.Dr, 450)
                    }), results.Journals[3], "A random journal should be correct");

            Assert.AreEqual("1234", results.TotalResults);
        }      
        
        
        [Test]
        public void EmptySpreadsheetReturnsNoJournals()
        {
            var results = GetAllJournalsFromSearch(new ExcelImportMapping
            {
                SheetData = new HeaderRowData
                {
                    Filename = ".\\ExcelSampleData.xls",
                    Sheet = 0,
                    UseHeaderRow = true,
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
            }, 1);

            Assert.AreEqual("0", results.TotalResults);
            CollectionAssert.IsEmpty(results.Journals);
        }

        private static SearchResponse GetAllJournalsFromSearch(ExcelImportMapping importMapping, int pageNumber)
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder();
            SearchResponse results;
            using (var scope = builder.Build())
            {
                var controller = scope.Resolve<ExcelSessionController>();
                controller.ExcelLogin(importMapping);

                var searchController = scope.Resolve<SearchController>();
                results =
                    searchController.UserSearch(
                        new SearchRequest<UserParameters>(
                            new SearchWindow<UserParameters>(new UserParameters("Steve"),
                                new DateRange(DateTime.MinValue, DateTime.MaxValue)), pageNumber));
            }
            return results;
        }
    }

}
