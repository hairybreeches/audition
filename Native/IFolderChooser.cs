using System.Threading.Tasks;

namespace Native
{
    public interface IFolderChooser
    {
        Task<string> GetFolder();
    }
}