using System.Linq;
using Model;
using Model.Accounting;
using NSubstitute;
using Xero;

namespace Tests.Mocks
{
    public class MockXeroRepositoryFactory
    {
        public static IRepositoryFactory Create(params Journal[] journals)
        {
            var repository = Substitute.For<IFullRepository>();
            repository.Journals.Returns(journals.Select(x => JournalExtensions.ToXeroJournal(x)).AsQueryable());
            var factory = Substitute.For<IRepositoryFactory>();
            factory.CreateRepository().Returns(repository);
            return factory;
        }
    }
}