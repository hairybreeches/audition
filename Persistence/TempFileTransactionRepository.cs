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
    public class TempFileTransactionRepository : ITransactionRepository, IDisposable
    {
        private readonly IFileSystem fileSystem;
        private string filename;

        public TempFileTransactionRepository(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            GenerateNewTempFile();
        }

        private void GenerateNewTempFile()
        {
            filename = Path.GetTempFileName();
        }

        public IQueryable<Transaction> GetTransactions()
        {
            return Transactions.AsQueryable();
        }

        private IEnumerable<Transaction> Transactions
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
        
        public ITransactionRepository UpdateTransactions(IEnumerable<Transaction> transactions)
        {
            var importedJournal = false;
            using(var writer = fileSystem.OpenFileToWrite(filename))
            foreach (var journal in transactions)
            {
                importedJournal = true;
                writer.WriteLine(JsonConvert.SerializeObject(journal));
            }

            if (!importedJournal)
            {
                throw new NoTransactionsException("Successfully opened accounts, but there seemed to be no journal entries");
            }
            return this;
        }

        public void ClearTransactions()
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
