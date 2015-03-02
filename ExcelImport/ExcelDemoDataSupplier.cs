using System.Collections.Generic;
using System.IO;

namespace ExcelImport
{
    public class ExcelDemoDataSupplier
    {
        public IEnumerable<string> GetDemoDataLocations()
        {
            return new[]
            {
                Path.GetFullPath(".\\ExampleSage50Export.xlsx"),
                Path.GetFullPath(".\\ComplexSage50Export.xls"),
            };
        }
    }
}