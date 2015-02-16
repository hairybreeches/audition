using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace Native
{
    public class FileSaveChooser : IFileSaveChooser
    {
        private readonly TaskFactory<ExportResult> taskFactory;

        public FileSaveChooser(TaskFactory<ExportResult> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public async Task<ExportResult> GetFileSaveLocation()
        {
            return await taskFactory.StartNew(GetValue);
        }

        private static ExportResult GetValue()
        {
            using (var dialog = new SaveFileDialog
            {
                Filter = "Excel spreadsheet|*.csv;*.xlsx;*.xls|All files|*.*"

            })
            {
                dialog.ShowDialog();

                if (!String.IsNullOrWhiteSpace(dialog.FileName))
                {
                    return ExportResult.Success(dialog.FileName);
                }

                return ExportResult.Incomplete();
                
            }
        }
    }
}