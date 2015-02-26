using Microsoft.Office.Interop.Excel;

namespace ExcelExport
{
    class ColumnDoesNotExistsFormatter : IExcelColumnFormatter
    {
        public int FormatColumn(Worksheet worksheet, int columnIndex)
        {
            return columnIndex;
        }
    }
}