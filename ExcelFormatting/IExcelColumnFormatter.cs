using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public interface IExcelColumnFormatter
    {
        void FormatColumn(Worksheet worksheet, int columnIndex);
    }
}