using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public class DateColumnFormatter : IExcelColumnFormatter
    {
        public void FormatColumn(Worksheet worksheet, int columnIndex)
        {
            var range = GetColumnDataCells(worksheet, columnIndex);
            range.NumberFormat = "DD/MM/YYYY";
        }

        private Range GetColumnDataCells(Worksheet sheet, int columnIndex)
        {
            return (Range) sheet.Columns[columnIndex];
        }
    }
}