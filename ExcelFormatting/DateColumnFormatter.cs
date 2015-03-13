using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public class DateColumnFormatter : IExcelColumnFormatter
    {
        public int FormatColumn(Worksheet worksheet, int columnIndex)
        {
            var range = GetColumnDataCells(worksheet, columnIndex);
            range.NumberFormat = "DD/MM/YYYY";
            return columnIndex + 1; 
        }

        private Range GetColumnDataCells(Worksheet sheet, int columnIndex)
        {
            return (Range) sheet.Columns[columnIndex];
        }
    }
}