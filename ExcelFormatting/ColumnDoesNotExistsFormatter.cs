using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public class ColumnDoesNotExistsFormatter : IExcelColumnFormatter
    {
        public int FormatColumn(Worksheet worksheet, int columnIndex)
        {
            return columnIndex;
        }
    }
}