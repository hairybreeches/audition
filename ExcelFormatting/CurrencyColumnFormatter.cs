using System;
using Microsoft.Office.Interop.Excel;

namespace ExcelFormatting
{
    public class CurrencyColumnFormatter : IExcelColumnFormatter
    {
        public void FormatColumn(Func<Range> columnGetter)
        {
            var column = columnGetter();
            column.Style = "Currency";
            column.NumberFormat = "#,##0.00";

        }
    }
}