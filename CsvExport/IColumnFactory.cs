using System.Collections.Generic;
using Capabilities;
using Model;

namespace CsvExport
{
    public interface IColumnFactory
    {
        ICsvColumn GetColumn(ICollection<DisplayFieldName> availableFields);
    }
}