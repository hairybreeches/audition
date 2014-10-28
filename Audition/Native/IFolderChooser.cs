using System.Threading.Tasks;

namespace Audition.Native
{
    public interface IFolderChooser
    {
        Task<string> GetFolder();
    }
}