using Audition.Controllers;
using Autofac;
using Native;
using NSubstitute;
using NUnit.Framework;
using Persistence;
using Sage50;
using Sage50.Parsing;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class MemoryUsageTests
    {
        [Test]
        public void JournalEnumerableNotEvaluatedOutsideRepository()
        {
            Assert.DoesNotThrow(() => LoginAndAddJournals(Substitute.For<IJournalRepository>()));
        }    
        
        [Test]
        public void JournalEnumerableEvaluatedInsideRepository()
        {
            Assert.Throws<EnumerableEvaluatedException>(() => LoginAndAddJournals(new TempFileJournalRepository(new FileSystem())));
        }

        private static void LoginAndAddJournals(IJournalRepository journalRepository)
        {
            var builder = AutofacConfiguration.CreateDefaultContainerBuilder();
            builder.RegisterType<ConnectionFactoryWhichThrowsWhenReaderIsEvaluated>().As<ISage50ConnectionFactory>();
            builder.Register(_ => journalRepository).As<IJournalRepository>();
            builder.Register(_ => Substitute.For<INominalCodeLookupFactory>()).As<INominalCodeLookupFactory>();

            using (var lifetime = builder.Build())
            using (var request = lifetime.BeginRequestScope())
            {
                var controller = request.Resolve<Sage50SessionController>();
                controller.Login(new Sage50LoginDetails());
            }
        }
    }
}
