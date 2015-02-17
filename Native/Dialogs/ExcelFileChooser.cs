using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Native.Dialogs
{
    public class ExcelFileChooser : IExcelFileChooser
    {
        private readonly TaskFactory<string> taskFactory;

        public ExcelFileChooser(TaskFactory<string> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public async Task<string> GetExcelFile(string startLocation)
        {
            return await taskFactory.StartNew(() => GetValue(startLocation));
        }

        private static string GetValue(string startLocation)
        {
            using (var dialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(startLocation),
                FileName = startLocation,
                Filter = "Excel spreadsheet|*.csv;*.xlsx;*.xls|All files|*.*"
            })
            {
                dialog.ShowDialog();                
                return dialog.FileName;
            }            
        }

    }
}