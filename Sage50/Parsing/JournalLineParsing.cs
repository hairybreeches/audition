using System;
using System.Data;
using System.Data.Odbc;
using Model.Accounting;

namespace Sage50.Parsing
{
    /// <summary>
    /// Knows how to turn the raw IDataRecord from the db into a SageJournalLine
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    static class JournalLineParsing
    {
        public static SageJournalLine CreateJournalLine(IDataRecord record)
        {
            return CreateJournalLine(
                GetField<int>(record, 0, "TRAN_NUMBER"),
                GetField<string>(record, 7, "USER_NAME"),
                GetField<DateTime>(record, 2, "DATE"),
                GetField<DateTime>(record, 31, "RECORD_CREATE_DATE"),
                GetField<string>(record, 4, "NOMINAL_CODE"),
                GetField<Double>(record, 20, "AMOUNT"), 
                GetField<string>(record, 8, "DETAILS")
                );
        }

        private static SageJournalLine CreateJournalLine(int transactionId, string username, DateTime journalDate, DateTime creationTime, string nominalCode, double rawAmount, string description)
        {
            JournalType type;
            decimal amount;

            if (rawAmount < 0)
            {
                type = JournalType.Cr;
                amount = -1 * (Decimal)rawAmount;
            }
            else
            {
                type = JournalType.Dr;
                amount = (Decimal)rawAmount;
            }

            return new SageJournalLine(transactionId, username, journalDate, creationTime, nominalCode, amount, type, description);
        }

        private static T GetField<T>(IDataRecord record, int index, string fieldName)
        {
            var actualFieldName = record.GetName(index);
            if (actualFieldName != fieldName)
            {
                throw new SageDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. Column {0} was {1}, expected {2}", index, actualFieldName, fieldName));
            }

            var fieldValue = record[index];

            if (!(fieldValue is T))
            {
                throw new SageDataFormatUnexpectedException(String.Format("Unrecognised data schema. {0} was {1}, expected {2}", fieldName, fieldValue.GetType(), typeof(T)));
            }

            return (T)fieldValue;
        }
    }
}