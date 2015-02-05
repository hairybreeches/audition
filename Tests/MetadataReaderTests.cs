using ExcelImport;
using Model;
using Native;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MetadataReaderTests
    {
        private string examplesage50export = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx";

        [Test]
        public void WithoutHeaderRowHeaderReaderReturnsLetteredListOfCorrectSize()
        {
            var reader = CreateReader();
            CollectionAssert.AreEqual(new[]
            {
                "Column A",
                "Column B",
                "Column C",
                "Column D",
                "Column E",
                "Column F",
                "Column G",
                "Column H",
                "Column I",
                "Column J",
                "Column K",
                "Column L",
                "Column M",
                "Column N",
                "Column O",
                "Column P",
                "Column Q",
                "Column R",
                "Column S",
                "Column T",
            },
                reader.ReadHeaders(new SheetMetadata
                {
                    Filename = examplesage50export,
                    UseHeaderRow = false
                }));
        }
        
        [Test]
        public void WitheaderRowHeaderReaderReturnsFirstCell()
        {
            var reader = CreateReader();
            CollectionAssert.AreEqual(new[]
            {
                "No",
                "Type",
                "Account",
                "Nominal",
                "Dept",
                "Details",
                "Date",
                "Ref",
                "Ex.Ref",
                "Net",
                "Tax",
                "T/C",
                "Paid",
                "Amount Paid",
                "Bank",
                "Bank Rec Date",
                "VAT",
                "VAT Rtn No.",
                "VAT Rec Date",
                "User Name",
            },
                reader.ReadHeaders(new SheetMetadata
                {
                    Filename = examplesage50export,
                    UseHeaderRow = true
                }));
        }

        [Test]
        public void CanReturnSheetNames()
        {
            var sheetNames = CreateReader().ReadSheets(examplesage50export);
            CollectionAssert.AreEqual(new[] { "Sage 50 journals export", "Unrelated data" }, sheetNames);
        }

        private static MetadataReader CreateReader()
        {
            return new MetadataReader(new ExcelColumnNamer(), new ExcelToSqlDataConverter(new FileSystem()));
        }
    }
}
