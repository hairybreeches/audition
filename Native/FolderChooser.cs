using System.Threading.Tasks;
using System.Windows.Forms;

namespace Native
{
    public class FolderChooser : IFolderChooser
    {
        private readonly TaskFactory<string> taskFactory;

        public FolderChooser(TaskFactory<string> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public async Task<string> GetFolder(string startFolder)
        {
            return await taskFactory.StartNew(() =>GetValue(startFolder));
        }

        private static string GetValue(string startFolder)
        {
            using (var dialog = new FolderBrowserDialog
            {
                SelectedPath = startFolder
            })
            {
                dialog.ShowDialog();
                return dialog.SelectedPath;
            }            
        }
    }
}