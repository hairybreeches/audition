using System;
using System.Collections.Generic;
using System.Web.Http;
using ExcelImport;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class ExcelSessionController : ApiController
    {
        private readonly MetadataReader metadataReader;
        private readonly ExcelDataFileStorage dataFileStorage;
        private readonly ExcelJournalReader journalReader;
        private readonly LoginSession session;
        private readonly FieldLookupInterpreter lookupInterpreter;

        public ExcelSessionController(MetadataReader metadataReader, ExcelDataFileStorage dataFileStorage, ExcelJournalReader journalReader, LoginSession session, FieldLookupInterpreter lookupInterpreter)
        {
            this.metadataReader = metadataReader;
            this.dataFileStorage = dataFileStorage;
            this.journalReader = journalReader;
            this.session = session;
            this.lookupInterpreter = lookupInterpreter;
        }

        [Route(Routing.ExcelLogin)]
        [HttpPost]
        public void ExcelLogin(ExcelImportMapping importMapping)
        {
            dataFileStorage.StoreUsage(importMapping);            
            session.Login(lookupInterpreter.CreateSearcherFactory(importMapping.Lookups), journalReader.ReadJournals(importMapping));
        }    
        
        
        [Route(Routing.GetExcelHeaders)]
        [HttpPost]
        public IEnumerable<string> GetHeaders(SheetMetadata sheetMetadata)
        {
            return metadataReader.ReadHeaders(sheetMetadata);
        }

        [Route(Routing.GetExcelSheets)]
        [HttpGet]
        public IEnumerable<string> GetSheets(String filename)
        {
            return metadataReader.ReadSheets(filename);
        }

    }
}
