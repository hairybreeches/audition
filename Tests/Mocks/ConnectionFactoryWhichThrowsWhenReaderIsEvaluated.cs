using System.Data;
using System.Data.Common;
using NSubstitute;
using Sage50;

namespace Tests.Mocks
{
    public class ConnectionFactoryWhichThrowsWhenReaderIsEvaluated : ISage50ConnectionFactory
    {
        public DbConnection OpenConnection(Sage50LoginDetails loginDetails)
        {

            var reader = Substitute.For<DbDataReader>();
            reader.Read().Returns(_ => { throw new EnumerableEvaluatedException(); });

            var command = Substitute.For<DbCommand>();
            command.ExecuteReader(Arg.Any<CommandBehavior>()).Returns(reader);

            var connection = Substitute.For<DbConnection>();
            connection.CreateCommand().Returns(command);

            return connection;
        }
    }
}