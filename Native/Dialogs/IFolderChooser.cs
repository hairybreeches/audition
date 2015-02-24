using System.Threading.Tasks;

namespace Native.Dialogs
{
    public interface IFolderChooser
    {
        Task<string> GetFolder(string startFolder);
    }
}