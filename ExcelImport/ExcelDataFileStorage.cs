using UserData;

namespace ExcelImport
{
    public class ExcelDataFileStorage : DataLocationStorage
    {
        public ExcelDataFileStorage(IUserDetailsStorage userDetailsStorage, ExcelDemoDataSupplier demoDataSupplier)
            :base(userDetailsStorage, demoDataSupplier, details => details.ExcelFiles, details => details.AddExcelFile)
        {
        }
    }
}