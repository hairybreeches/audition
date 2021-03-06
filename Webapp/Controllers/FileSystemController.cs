﻿using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using Native;
using Native.Dialogs;
using Webapp.Requests;

namespace Webapp.Controllers
{
    public class FileSystemController : RedirectController
    {
        private readonly IFolderChooser folderChooser;
        private readonly IExcelFileChooser excelFileChooser;

        public FileSystemController(IFolderChooser folderChooser, IExcelFileChooser excelFileChooser)
        {
            this.folderChooser = folderChooser;
            this.excelFileChooser = excelFileChooser;
        }

        [HttpPost]
        [Route(Routing.Openfile)]
        public IHttpActionResult OpenFile(OpenFileRequest request)
        {
            Process.Start(request.FileName);
            return Ok();
        }
        
        [HttpGet]
        [Route(Routing.ChooseDirectory)]
        public async Task<IHttpActionResult> ChooseFolder(string start)
        {
            var folder = await folderChooser.GetFolder(start);
            return Ok(folder);
        }    
        
        [HttpGet]
        [Route(Routing.ChooseExcelFile)]
        public async Task<IHttpActionResult> ChooseExcelFile(string start)
        {
            var folder = await excelFileChooser.GetExcelFile(start);
            return Ok(folder);            
        }

        
    }
}
