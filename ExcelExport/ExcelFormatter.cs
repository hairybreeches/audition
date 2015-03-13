using System;
using System.Collections.Generic;
using ExcelFormatting;
using Microsoft.Office.Interop.Excel;

namespace ExcelExport
{
    public class ExcelFormatter : IDisposable
    {
        private readonly Workbook workbook;
        private readonly Lazy<Worksheet> lazyWorksheet;

        public ExcelFormatter(Workbook workbook)
        {
            this.workbook = workbook;
            this.lazyWorksheet = new Lazy<Worksheet>(GetWorksheet);
        }

        private Worksheet Sheet
        {
            get { return lazyWorksheet.Value; }
        }

        private Worksheet GetWorksheet()
        {
            var worksheet = (Worksheet) workbook.Worksheets[1];

            if (worksheet == null)
            {
                throw new ExcelInteropException(
                    "Worksheet could not be created. Check that your office installation and project references are correct.");
            }

            return worksheet;
        }

        public void MergeRow(int rowIndex)
        {            
            var cellsInRow = (Range) Sheet.Rows[rowIndex];
            cellsInRow.Merge();
        }   
        
        public void AutosizeColumns()
        {
            Sheet.UsedRange.Columns.AutoFit();
        }

        public void SaveAs(string filename)
        {
            workbook.SaveAs(filename, XlFileFormat.xlOpenXMLWorkbook);
        }

        public void Dispose()
        {
            workbook.Close(false);
        }

        public void ApplyFiltersToRow(int rowIndex)
        {
            Sheet.Application.ActiveWindow.SplitRow = rowIndex;
            Sheet.Application.ActiveWindow.FreezePanes = true;
            // Now apply autofilter
            var headerRow = (Range) Sheet.Rows[rowIndex];
            headerRow.AutoFilter(1,
                                Type.Missing,
                                XlAutoFilterOperator.xlAnd,
                                Type.Missing,
                                true);
        }

        public void NameSheet(string name)
        {
            Sheet.Name = name;
        }

        public void FormatColumns(IEnumerable<IExcelColumnFormatter> excelColumnFormatters, int headerRows)
        {
            var columnIndex = 1;
            foreach (var excelColumnFormatter in excelColumnFormatters)
            {
                excelColumnFormatter.FormatColumn(Sheet, columnIndex);
                columnIndex ++;
            }
        }
    }
}