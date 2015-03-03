using System.Collections.Generic;
using System.IO;
using UserData;

namespace ExcelImport
{
    public class ExcelDemoDataSupplier : IDemoDataSupplier
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