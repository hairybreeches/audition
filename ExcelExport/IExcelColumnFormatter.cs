using System.Runtime.Remoting.Messaging;
using Microsoft.Office.Interop.Excel;

namespace ExcelExport
{
    public interface IExcelColumnFormatter
    {
        int FormatColumn(Worksheet worksheet, int columnIndex);
    }
}