using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Web.Http;
using System.Windows.Forms;

namespace Audition.Controllers
{
    public class FileSystemController : RedirectController
    {
        private readonly TaskFactory<string> taskFactory;

        public FileSystemController(TaskFactory<string> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        [HttpGet]
        [Route("api/choosefile")]        
        public async Task<string> FileChooserDialog(string current)
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
