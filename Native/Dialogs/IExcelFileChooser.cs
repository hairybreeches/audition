using System.Threading.Tasks;

namespace Native.Dialogs
{
    public interface IExcelFileChooser
    {
        Task<string> GetExcelFile(string startLocation);
    }
}