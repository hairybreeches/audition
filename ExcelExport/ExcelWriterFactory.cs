using CsvExport;
using Microsoft.Office.Interop.Excel;

namespace ExcelExport
{
    public class ExcelFileOpener
    {
        public ExcelFormatter OpenFile(string filename)
        {
            var xlApp = new Application
            {
                Visible = false,
                DisplayAlerts = false
            };

            return new ExcelFormatter(xlApp.Workbooks.Open(filename));
        }
    }
}