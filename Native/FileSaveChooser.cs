using System.Threading.Tasks;
using System.Windows.Forms;

namespace Native
{
    public class FileSaveChooser : IFileSaveChooser
    {
        private readonly TaskFactory<string> taskFactory;

        public FileSaveChooser(TaskFactory<string> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public async Task<string> GetFileSaveLocation()
        {
            return await taskFactory.StartNew(GetValue);
        }

        private static string GetValue()
        {
            using (var dialog = new SaveFileDialog
            {
                Filter = "Excel spreadsheet|*.csv;*.xlsx;*.xls|All files|*.*"

            })
            {
                dialog.ShowDialog();
                return dialog.FileName;
            }
        }
    }
}