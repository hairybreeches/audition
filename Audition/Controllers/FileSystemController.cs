﻿using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Audition.Chromium;
using Audition.Native;

namespace Audition.Controllers
{
    public class FileSystemController : RedirectController
    {
        private readonly IFolderChooser folderChooser;

        public FileSystemController(IFolderChooser folderChooser)
        {
            this.folderChooser = folderChooser;
        }

        [HttpGet]
        [Route(Routing.Openfile)]
        public IHttpActionResult OpenFile(string fileName)
        {
            Process.Start(fileName);
            return Ok();
        }
        
        [HttpGet]
        [Route(Routing.ChooseDirectory)]
        public async Task<IHttpActionResult> OpenFile()
        {
            var folder = await folderChooser.GetFolder();
            return Ok(folder);
        }

        
    }
}
