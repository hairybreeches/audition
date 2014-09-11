using System.Threading.Tasks;
using System.Windows.Forms;
using Audition.Controllers;

namespace Audition.Native
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
            var fbd = new SaveFileDialog
            {
                Filter = "Excel spreadsheet|*.csv;*.xlsx;*.xls|PDF document|*.pdf|All files|*.*"

            };

            fbd.ShowDialog();

            return fbd.FileName;
        }
    }
}