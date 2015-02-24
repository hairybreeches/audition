using System.Collections.Generic;
using Model;

namespace ExcelExport
{
    public interface IFormatterFactory
    {
        IExcelColumnFormatter GetFormatter(ICollection<DisplayField> availableFields);
    }
}