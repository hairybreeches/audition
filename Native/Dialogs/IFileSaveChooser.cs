using System.Threading.Tasks;
using Model;

namespace Native.Dialogs
{
    public interface IFileSaveChooser
    {
        Task<ExportResult> GetFileSaveLocation();
    }
}