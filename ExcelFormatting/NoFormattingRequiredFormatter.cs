using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public class NoFormattingRequiredFormatter : IExcelColumnFormatter
    {
        public int FormatColumn(Worksheet worksheet, int columnIndex)
        {
            return columnIndex + 1;
        }
    }
}
