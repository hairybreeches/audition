using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public interface IExcelColumnFormatter
    {
        int FormatColumn(Worksheet worksheet, int columnIndex);
    }
}