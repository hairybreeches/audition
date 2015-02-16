using System.Threading.Tasks;
using Model;

namespace Native
{
    public interface IFileSaveChooser
    {
        Task<ExportResult> GetFileSaveLocation();
    }
}