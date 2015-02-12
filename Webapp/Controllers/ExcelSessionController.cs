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
        private readonly Session.Session session;
        private readonly ISearcherFactoryFactory lookupInterpreter;

        public ExcelSessionController(MetadataReader metadataReader, ExcelDataFileStorage dataFileStorage, ExcelJournalReader journalReader, Session.Session session, ISearcherFactoryFactory lookupInterpreter)
        {
            this.metadataReader = metadataReader;
            this.dataFileStorage = dataFileStorage;
            this.journalReader = journalReader;
            this.session = session;
            this.lookupInterpreter = lookupInterpreter;
        }

        [Route(Routing.ExcelImport)]
        [HttpPost]
        public void ExcelImport(ExcelImportMapping importMapping)
        {
            dataFileStorage.StoreUsage(importMapping);            
            session.ImportData(lookupInterpreter.CreateSearcherFactory(importMapping.Lookups), journalReader.ReadJournals(importMapping));
        }    

        [Route(Routing.GetExcelSheets)]
        [HttpGet]
        public IEnumerable<SheetMetadata> GetSheets(String filename)
        {
            return metadataReader.ReadSheets(filename);
        }

    }
}
