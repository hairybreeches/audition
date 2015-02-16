using System;
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

        public IQueryable<Transaction> GetJournals()
        {
            return Journals.AsQueryable();
        }

        private IEnumerable<Transaction> Journals
        {
            get
            {
                using (var reader = fileSystem.OpenFileToRead(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        yield return JsonConvert.DeserializeObject<Transaction>(reader.ReadLine());
                    }
                }
            }
        } 
        
        public IJournalRepository UpdateJournals(IEnumerable<Transaction> journals)
        {
            var importedJournal = false;
            using(var writer = fileSystem.OpenFileToWrite(filename))
            foreach (var journal in journals)
            {
                importedJournal = true;
                writer.WriteLine(JsonConvert.SerializeObject(journal));
            }

            if (!importedJournal)
            {
                throw new NoJournalsException("Successfully opened accounts, but there seemed to be no journal entries");
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
