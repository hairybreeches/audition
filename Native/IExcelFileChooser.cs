using System.Threading.Tasks;

namespace Native
{
    public interface IExcelFileChooser
    {
        Task<string> GetExcelFile(string startLocation);
    }
}