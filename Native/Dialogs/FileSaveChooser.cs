using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace Native.Dialogs
{
    public class FileSaveChooser : IFileSaveChooser
    {
        private readonly TaskFactory<ExportResult> taskFactory;
        private readonly DialogShower shower;

        public FileSaveChooser(TaskFactory<ExportResult> taskFactory, DialogShower shower)
        {
            this.taskFactory = taskFactory;
            this.shower = shower;
        }

        public async Task<ExportResult> GetFileSaveLocation()
        {
            return await taskFactory.StartNew(GetValue);
        }

        private ExportResult GetValue()
        {
            using (var dialog = new SaveFileDialog
            {
                Filter = "Excel spreadsheet|*.csv;*.xlsx;*.xls|All files|*.*"

            })
            {
                shower.ShowDialog(dialog);

                if (!String.IsNullOrWhiteSpace(dialog.FileName))
                {
                    return ExportResult.Success(dialog.FileName);
                }

                return ExportResult.Incomplete();
                
            }
        }
    }
}