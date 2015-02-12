using System.Data.Common;

namespace Sage50
{
    public interface ISage50ConnectionFactory
    {
        DbConnection OpenConnection(Sage50ImportDetails importDetails);
    }
}