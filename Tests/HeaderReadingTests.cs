﻿using ExcelImport;
using Native;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HeaderReadingTests
    {
        [Test]
        public void WithoutHeaderRowHeaderReaderReturnsLetteredListOfCorrectSize()
        {
            var reader = new HeaderReader(new FileSystem(), new ExcelColumnNamer());
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
                reader.ReadHeaders(new HeaderRowData
                {
                    Filename = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx",
                    UseHeaderRow = false
                }));
        }
        
        [Test]
        public void WitheaderRowHeaderReaderReturnsFirstCell()
        {
            var reader = new HeaderReader(new FileSystem(), new ExcelColumnNamer());
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
                reader.ReadHeaders(new HeaderRowData
                {
                    Filename = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx",
                    UseHeaderRow = true
                }));
        }

    }
}