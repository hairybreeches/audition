using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public class NoFormattingRequiredFormatter : IExcelColumnFormatter
    {
        public void FormatColumn(Worksheet worksheet, int columnIndex)
        {
        }
    }
}
