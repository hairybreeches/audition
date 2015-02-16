using System;
using Autofac;
using ExcelImport;
using Model;
using Model.Accounting;
using Model.Responses;
using Model.Time;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;
using SqlImport;
using Tests;
using Webapp.Controllers;
using Webapp.Requests;

namespace SystemTests
{
    [TestFixture]
    public class ExcelImportTests
    {
        [Test]
        public void CanImportDataFromExcelCombiningOnId()
        {
            var result = GetAllTransactionsFromSearch(new ExcelImportMapping
            {
                SheetDescription = new DataSet
                {
                    Filename = "..\\..\\..\\ExcelImport\\ComplexSage50Export.xls",
                    Sheet = 0,
                    UseHeaderRow = true,
                },
                Lookups = new FieldLookups
                (
                    accountCode : 4,
                    accountName : -1,
                    amount : 20,
                    created : 31,
                    description : 8,
                    transactionDate : 2,
                    username : 7,
                    id:0
                )
            }, 1);

            Assert.AreEqual("1217", result.TotalResults, "We should get all the transactions back");

            Assert.AreEqual(new Transaction("8", new DateTimeOffset(2010, 4, 27, 17, 16, 57, TimeSpan.FromHours(1)), new DateTime(2013, 12, 31), "MANAGER", "Opening Balance", new[]
            {
                new LedgerEntry("1100", null, LedgerEntryType.Cr, 0.05m), 
                new LedgerEntry("9998", null, LedgerEntryType.Dr, 0.05m), 
                new LedgerEntry("2200", null, LedgerEntryType.Dr, 0)
            }), result.Transactions[7], "A random transaction should be correct");
        }

        [Test]
        //sage 50 exports dates as strings and our sample data has GB dates
        [SetCulture("en-GB")]
        public void GetHelpfulErrorMessageWhenCannotParseDate()
        {
            var exception =
                Assert.Throws<SqlDataFormatUnexpectedException>(() => GetAllTransactionsFromSearch(new ExcelImportMapping
                {
                    SheetDescription = new DataSet()
                    {
                        Filename = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx",
                        
                        Sheet= 0,
                        UseHeaderRow = true,
                    },
                    Lookups = new FieldLookups
                    (
                        accountCode: 3,
                        accountName: -1,
                        amount: 9,
                        created: 3,
                        description: 5,
                        transactionDate: 6,
                        username: 19,
                        id: -1
                    )
                }, 1));

            Assert.AreEqual("Could not read transaction entry time of row 3: Could not interpret value '9998' from column D as a date", exception.Message);
        }      
        
        [Test]
        //sage 50 exports dates as strings and our sample data has GB dates
        [SetCulture("en-GB")]
        public void CanImportExcelData()
        {
            var results = GetAllTransactionsFromSearch(new ExcelImportMapping
            {
                SheetDescription = new DataSet
                {
                    Filename = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx",
                    Sheet = 0,
                    UseHeaderRow = true,
                },
                Lookups = new FieldLookups
                (
                    accountCode : 3,
                    accountName : -1,
                    amount : 9,
                    created : -1,
                    description : 5,
                    transactionDate : 6,
                    username : 19,
                    id:-1
                )
            }, 6);

            Assert.AreEqual(new Transaction("62", default(DateTime), new DateTime(2013, 1, 30), "MANAGER", "Telephone Accrual",
                    new[]
                    {
                        new LedgerEntry("7502", null, LedgerEntryType.Dr, 50)
                    }), results.Transactions[9], "A random transaction should be correct");

            Assert.AreEqual("1234", results.TotalResults);
        }      
        
        
        [Test]
        [ExpectedException(typeof(NoTransactionsException))]
        public void EmptySpreadsheetThrowsNoTransactionsException()
        {
            GetAllTransactionsFromSearch(new ExcelImportMapping
            {
                SheetDescription = new DataSet
                {
                    Filename = ".\\ExcelSampleData.xls",
                    Sheet = 0,
                    UseHeaderRow = true,
                },
                Lookups = new FieldLookups
                (
                    accountCode : 3,
                    accountName : -1,
                    amount : 9,
                    created : -1,
                    description : 5,
                    transactionDate : 6,
                    username : 19,
                    id: -1
                )
            }, 1);

        }

        private static SearchResponse GetAllTransactionsFromSearch(ExcelImportMapping importMapping, int pageNumber)
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder()
                .WithNoLicensing();
            SearchResponse results;
            using (var scope = builder.Build())
            {
                var controller = scope.Resolve<ExcelSessionController>();
                controller.ExcelImport(importMapping);

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
