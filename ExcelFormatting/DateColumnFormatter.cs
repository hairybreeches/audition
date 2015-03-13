using System;
using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public class DateColumnFormatter : IExcelColumnFormatter
    {
        public void FormatColumn(Func<Range> columnGetter)
        {
            var range = columnGetter();
            range.NumberFormat = "DD/MM/YYYY";
        }
    }
}