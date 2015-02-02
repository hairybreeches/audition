using ExcelImport;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ExcelColumnNamerTests
    {
        [TestCase(0, ExpectedResult = "Column A")]
        [TestCase(1, ExpectedResult = "Column B")]
        [TestCase(2, ExpectedResult = "Column C")]
        [TestCase(3, ExpectedResult = "Column D")]
        [TestCase(9, ExpectedResult = "Column J")]
        [TestCase(10, ExpectedResult = "Column K")]
        [TestCase(23, ExpectedResult = "Column X")]
        [TestCase(24, ExpectedResult = "Column Y")]
        [TestCase(25, ExpectedResult = "Column Z")]
        [TestCase(26, ExpectedResult = "Column AA")]
        [TestCase(32, ExpectedResult = "Column AG")]
        [TestCase(51, ExpectedResult = "Column AZ")]
        [TestCase(52, ExpectedResult = "Column BA")]
        [TestCase(53, ExpectedResult = "Column BB")]
        [TestCase(701, ExpectedResult = "Column ZZ")]
        [TestCase(702, ExpectedResult = "Column AAA")]
        [TestCase(703, ExpectedResult = "Column AAB")]
        [TestCase(728, ExpectedResult = "Column ABA")]
        public string ReturnsExpectedColumnNamesForIndexes(int columnIndex)
        {
            return new ExcelColumnNamer().GetColumnName(columnIndex);
        }
    }
}
