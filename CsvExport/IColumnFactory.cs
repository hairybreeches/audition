using System.Collections.Generic;
using Model;

namespace CsvExport
{
    public interface IColumnFactory
    {
        ICsvColumn GetColumn(ICollection<DisplayField> availableFields);
    }
}