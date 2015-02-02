using ExcelImport;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ExcelColumnNamerTests
    {
        [TestCase(0, ExpectedResult = "A")]
        [TestCase(1, ExpectedResult = "B")]
        [TestCase(2, ExpectedResult = "C")]
        [TestCase(3, ExpectedResult = "D")]
        [TestCase(9, ExpectedResult = "J")]
        [TestCase(10, ExpectedResult = "K")]
        [TestCase(23, ExpectedResult = "X")]
        [TestCase(24, ExpectedResult = "Y")]
        [TestCase(25, ExpectedResult = "Z")]
        [TestCase(26, ExpectedResult = "AA")]
        [TestCase(32, ExpectedResult = "AG")]
        [TestCase(51, ExpectedResult = "AZ")]
        [TestCase(52, ExpectedResult = "BA")]
        [TestCase(53, ExpectedResult = "BB")]
        [TestCase(701, ExpectedResult = "ZZ")]
        [TestCase(702, ExpectedResult = "AAA")]
        [TestCase(703, ExpectedResult = "AAB")]
        [TestCase(728, ExpectedResult = "ABA")]
        public string ReturnsExpectedColumnNamesForIndexes(int columnIndex)
        {
            return new ExcelColumnNamer().GetColumnName(columnIndex);
        }
    }
}
