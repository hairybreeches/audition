﻿using System.Collections.Generic;
using Model;
using Model.Accounting;

namespace CsvExport
{
    public interface ITransactionExporter
    {
        void Export(string description, IEnumerable<Transaction> transactions, string filename, IEnumerable<DisplayField> availableFields);
    }
}