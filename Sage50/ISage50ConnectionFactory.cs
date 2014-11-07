using System.Data.Odbc;

namespace Sage50
{
    public interface ISage50ConnectionFactory
    {
        OdbcConnection OpenConnection(Sage50LoginDetails loginDetails);
    }
}