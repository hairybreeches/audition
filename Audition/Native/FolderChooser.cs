using System.Threading.Tasks;
using System.Windows.Forms;

namespace Audition.Native
{
    public class FolderChooser : IFolderChooser
    {
        private readonly TaskFactory<string> taskFactory;

        public FolderChooser(TaskFactory<string> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public async Task<string> GetFolder()
        {
            return await taskFactory.StartNew(GetValue);
        }

        private static string GetValue()
        {
            var dialog = new FolderBrowserDialog();

            dialog.ShowDialog();

            return dialog.SelectedPath;
        }
    }
}