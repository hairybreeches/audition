using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using Model.Accounting;
using Model.Time;
using Native;
using Native.Disk;
using Newtonsoft.Json;

namespace Persistence
{
    public class TempFileTransactionRepository : ITransactionRepository, IDisposable
    {
        private readonly IFileSystem fileSystem;
        private TempFile file;

        public TempFileTransactionRepository(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            GenerateNewTempFile();
        }

        private void GenerateNewTempFile()
        {
            file = fileSystem.GetTempFile();
        }

        public IQueryable<Transaction> GetTransactions()
        {
            return Transactions.AsQueryable();
        }

        private IEnumerable<Transaction> Transactions
        {
            get
            {
                using (var reader = file.OpenFileToRead())
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
            var imported = false;
            using(var writer = file.OpenFileToWrite())
            foreach (var transaction in transactions)
            {
                imported = true;
                writer.WriteLine(JsonConvert.SerializeObject(transaction));
            }

            if (!imported)
            {
                throw new NoTransactionsException("Successfully opened accounts, but there seemed to be no transactions");
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
           using(file){}
        }

        public void Dispose()
        {
            DeleteTempFile();
        }
    }

   
}
