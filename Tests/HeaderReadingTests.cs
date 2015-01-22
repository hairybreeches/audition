using System;
using ExcelImport;
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
            reader.ReadHeaders("..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx"));
        }

    }
}
