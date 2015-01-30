﻿using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using SqlImport;

namespace ExcelImport
{
    public class ExcelJournalReader
    {
        private readonly ExcelDataConverter dataConverter;
        private readonly SqlJournalReader sqlJournalReader;

        public ExcelJournalReader(ExcelDataConverter dataConverter, SqlJournalReader sqlJournalReader)
        {
            this.dataConverter = dataConverter;
            this.sqlJournalReader = sqlJournalReader;
        }

        public IEnumerable<Journal> ReadJournals(ExcelImportMapping importMapping)
        {
            var sheetReader = dataConverter.GetSheet(importMapping.SheetData).CreateDataReader();            
            do
            {
                if (!sheetReader.Read())
                {
                    throw new NoJournalsException("Successfully opened the spreadsheet, but it appears to have no rows apart from header/empty rows");
                }
            } while (sheetReader.RowIsEmpty());

            return sqlJournalReader.GetJournals(sheetReader, importMapping.Lookups.ToJournalSchema());
        }
    }
}