using System.Threading.Tasks;
using System.Windows.Forms;

namespace Native.Dialogs
{
    public class FolderChooser : IFolderChooser
    {
        private readonly TaskFactory<string> taskFactory;
        private readonly DialogShower shower;

        public FolderChooser(TaskFactory<string> taskFactory, DialogShower shower)
        {
            this.taskFactory = taskFactory;
            this.shower = shower;
        }

        public async Task<string> GetFolder(string startFolder)
        {
            return await taskFactory.StartNew(() =>GetValue(startFolder));
        }

        private string GetValue(string startFolder)
        {
            using (var dialog = new FolderBrowserDialog
            {
                SelectedPath = startFolder
            })
            {
                shower.ShowDialog(dialog);

                return dialog.SelectedPath;
            }
        }
    }
}