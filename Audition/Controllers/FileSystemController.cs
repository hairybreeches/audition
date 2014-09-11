using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;

namespace Audition.Controllers
{
    public class FileSystemController : RedirectController
    {
        private readonly FileSaveChooser saveChooser;


        public FileSystemController(FileSaveChooser saveChooser)
        {
            this.saveChooser = saveChooser;            
        }

        [HttpGet]
        [Route("api/choosefile")]        
        public async Task<string> FileChooserDialog(string current)
        {                        
            return await saveChooser.GetFileSaveLocation(current);
        }

       

        [HttpGet]
        [Route("api/openfile")]
        public IHttpActionResult OpenFile(string fileName)
        {
            Process.Start(fileName);
            return Ok();
        }

        
    }
}
