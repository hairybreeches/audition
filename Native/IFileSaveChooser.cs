using System.Threading.Tasks;

namespace Native
{
    public interface IFileSaveChooser
    {
        Task<string> GetFileSaveLocation();
    }
}