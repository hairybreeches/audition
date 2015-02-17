using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Native.Dialogs
{
    public class ExcelFileChooser : IExcelFileChooser
    {
        private readonly TaskFactory<string> taskFactory;
        private readonly DialogShower shower;

        public ExcelFileChooser(TaskFactory<string> taskFactory, DialogShower shower)
        {
            this.taskFactory = taskFactory;
            this.shower = shower;
        }

        public async Task<string> GetExcelFile(string startLocation)
        {
            return await taskFactory.StartNew(() => GetValue(startLocation));
        }

        private string GetValue(string startLocation)
        {
            using (var dialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(startLocation),
                FileName = startLocation,
                Filter = "Excel spreadsheet|*.csv;*.xlsx;*.xls|All files|*.*"
            })
            {
                shower.ShowDialog(dialog);
                return dialog.FileName;
            }            
        }

    }
}