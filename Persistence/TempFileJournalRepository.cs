﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using Model.Accounting;
using Model.Time;
using Native;
using Newtonsoft.Json;

namespace Persistence
{
    public class TempFileJournalRepository : IJournalRepository, IDisposable
    {
        private readonly IFileSystem fileSystem;
        private string filename;

        public TempFileJournalRepository(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            GenerateNewTempFile();
        }

        private void GenerateNewTempFile()
        {
            filename = Path.GetTempFileName();
        }

        public IQueryable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return Journals.AsQueryable()
                .Where(x => period.Contains(x.JournalDate));
        }

        private IEnumerable<Journal> Journals
        {
            get
            {
                using (var reader = fileSystem.OpenFileToRead(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        yield return JsonConvert.DeserializeObject<Journal>(reader.ReadLine());
                    }
                }
            }
        } 
        
        public IJournalRepository UpdateJournals(IEnumerable<Journal> journals)
        {            
            using(var writer = fileSystem.OpenFileToWrite(filename))
            foreach (var journal in journals)
            {
                writer.WriteLine(JsonConvert.SerializeObject(journal));
            }

            return this;
        }

        public void ClearJournals()
        {
            DeleteTempFile();
            GenerateNewTempFile();
        }

        private void DeleteTempFile()
        {
            fileSystem.DeleteFile(filename);
        }

        public void Dispose()
        {
            DeleteTempFile();
        }
    }
}