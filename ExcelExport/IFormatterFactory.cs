using System.Collections.Generic;
using Capabilities;
using ExcelFormatting;
using Model;

namespace ExcelExport
{
    public interface IFormatterFactory
    {
        IExcelColumnFormatter GetFormatter(ICollection<DisplayFieldName> availableFields);
    }
}