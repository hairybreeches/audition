using System;

namespace ExcelImport
{
    public class ExcelColumnNamer
    {
        public string GetColumnName(int columnNumber)
        {
            return GetExcelColumnName(columnNumber + 1);
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = String.Empty;

            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
    }
}
