using System.Threading.Tasks;
using System.Windows.Forms;

namespace Audition.Controllers
{
    public class FileSaveChooser
    {
        private readonly TaskFactory<string> taskFactory;

        public FileSaveChooser(TaskFactory<string> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public async Task<string> GetFileSaveLocation(string current)
        {
            return await taskFactory.StartNew(() => GetValue(current));
        }

        private static string GetValue(string current)
        {
            var fbd = new SaveFileDialog
            {
                FileName = current,
                Filter = "Excel spreadsheet|*.csv;*.xlsx;*.xls|PDF document|*.pdf|All files|*.*"

            };

            fbd.ShowDialog();

            return fbd.FileName;
        }
    }
}