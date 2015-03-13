using System;
using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public interface IExcelColumnFormatter
    {
        void FormatColumn(Func<Range> columnGetter);
    }
}