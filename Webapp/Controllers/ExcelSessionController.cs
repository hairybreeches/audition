using System;
using System.Collections.Generic;
using System.Web.Http;
using ExcelImport;
using Model;

namespace Webapp.Controllers
{
    public class ExcelSessionController : ApiController
    {
        private readonly MetadataReader metadataReader;
        private readonly ExcelDataFileStorage dataFileStorage;

        public ExcelSessionController(MetadataReader metadataReader, ExcelDataFileStorage dataFileStorage)
        {
            this.metadataReader = metadataReader;
            this.dataFileStorage = dataFileStorage;
        }

        [Route(Routing.ExcelLogin)]
        [HttpPost]
        public void ExcelLogin(ExcelImportMapping importMapping)
        {
            dataFileStorage.StoreUsage(importMapping);
        }    
        
        
        [Route(Routing.GetExcelHeaders)]
        [HttpPost]
        public IEnumerable<string> GetHeaders(HeaderRowData headerRowData)
        {
            return metadataReader.ReadHeaders(headerRowData);
        }

        [Route(Routing.GetExcelSheets)]
        [HttpGet]
        public IEnumerable<string> GetSheets(String filename)
        {
            return metadataReader.ReadSheets(filename);
        }

    }
}
