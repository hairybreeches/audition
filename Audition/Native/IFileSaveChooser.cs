using System.Threading.Tasks;

namespace Audition.Native
{
    public interface IFileSaveChooser
    {
        Task<string> GetFileSaveLocation();
    }
}