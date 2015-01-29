using Autofac;
using ExcelImport;
using NUnit.Framework;
using Tests;
using Webapp.Controllers;

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
                        AccountCode = 2,
                        AccountName = -1,
                        Amount = 9,
                        Created = -1,
                        Description = 5,
                        JournalDate = 6,
                        Username = 19
                    }

                });
            }
        }
    }
}
