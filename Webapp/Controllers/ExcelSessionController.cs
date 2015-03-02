using System;
using System.Collections.Generic;
using System.Web.Http;
using Capabilities;
using ExcelImport;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class ExcelSessionController : ApiController
    {
        private readonly MetadataReader metadataReader;
        private readonly ExcelDataFileStorage dataFileStorage;
        private readonly ExcelJournalReader reader;
        private readonly Session.Session session;
        private readonly ISearcherFactoryFactory lookupInterpreter;

        public ExcelSessionController(MetadataReader metadataReader, ExcelDataFileStorage dataFileStorage, ExcelJournalReader reader, Session.Session session, ISearcherFactoryFactory lookupInterpreter)
        {
            this.metadataReader = metadataReader;
            this.dataFileStorage = dataFileStorage;
            this.reader = reader;
            this.session = session;
            this.lookupInterpreter = lookupInterpreter;
        }

        [Route(Routing.ExcelImport)]
        [HttpPost]
        public void ExcelImport(ExcelImportMapping importMapping)
        {
            if (importMapping.Lookups.TransactionDate < 0)
            {
                throw new ExcelMappingException(String.Format("The {0} must be mapped. Please select a column to map the {0} to and try again", MappingFields.TransactionDate));
            }
            dataFileStorage.StoreUsage(importMapping);            
            session.ImportData(lookupInterpreter.CreateSearcherFactory(importMapping.Lookups), reader.ReadJournals(importMapping));
        }    

        [Route(Routing.GetExcelSheets)]
        [HttpGet]
        public IEnumerable<SheetMetadata> GetSheets(String filename)
        {
            return metadataReader.ReadSheets(filename);
        }

    }
}
