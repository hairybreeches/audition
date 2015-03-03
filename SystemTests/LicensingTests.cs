using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Licensing;
using Model.Accounting;
using Model.Responses;
using Model.Time;
using Native;
using Native.RegistryAccess;
using Native.Time;
using NSubstitute;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;
using Tests;
using Tests.Mocks;
using Webapp.Controllers;
using Webapp.Requests;

namespace SystemTests
{
    [TestFixture]
    public class LicensingTests
    {

        public IEnumerable<TestCaseData> LicensingTestCases
        {
            get
            {
                yield return new TestCaseData(new DateTime(1999, 1, 1), new DateTime(1999, 1, 4), null)
                    .Returns(Enumerable.Empty<Transaction>())
                    .SetName("When the trial is still valid the search succeeds");

                yield return new TestCaseData(new DateTime(1999, 1, 1), new DateTime(1999, 1, 4), "steve")
                    .Returns(Enumerable.Empty<Transaction>())
                    .SetName("When the trial is not valid but there is a licence key the search succeeds");

                yield return new TestCaseData(new DateTime(1999, 1, 1), new DateTime(1999, 1, 31), null)
                    .Throws(typeof(UnlicensedException))
                    .SetName("When the trial is not valid and there is no licence key the search fails");
            }
        }


        [TestCaseSource("LicensingTestCases")]
        public IList<Transaction> CanExecuteSearchOnlyWhenLicenceValid(DateTime trialStartDate, DateTime currentDate, string licenceKey)
        {
            var mockRegistry = CreateRegistry(licenceKey, trialStartDate);
            return ExecuteSearch(currentDate, mockRegistry).Transactions;
        }

        private static SearchResponse ExecuteSearch(DateTime currentDate, ICurrentUserRegistry registry)
        {
            var containerBuilder = CreateContainerBuilder(currentDate, registry);
            using (var lifetime = containerBuilder.BuildSearchable(new[] { new Transaction(null, new DateTime(), null, null, String.Empty, String.Empty) }))
            {
                var controller = lifetime.Resolve<SearchController>();
                return controller.NominalCodesSearch(CreateSearchRequest());
            }
        }

        private ICurrentUserRegistry CreateRegistry(string licenceKey, DateTime trialStartDate)
        {
            var registry = new MockRegistry()
                .SetValue("SOFTWARE\\Audition\\Audition", "TrialStart", trialStartDate.Date.ToString());

            if (!String.IsNullOrEmpty(licenceKey))
            {
                registry.SetValue("SOFTWARE\\Audition\\Audition", "LicenceKey", licenceKey);
            }

            return registry;
        }

        private static ContainerBuilder CreateContainerBuilder(DateTime currentDate, ICurrentUserRegistry registry)
        {
            var containerBuilder = AutofacConfiguration.CreateDefaultContainerBuilder();
            containerBuilder.Register(_ => CreateClock(currentDate)).As<IClock>();
            containerBuilder.Register(_ => registry).As<ICurrentUserRegistry>();
            return containerBuilder;
        }

        private static IClock CreateClock(DateTime currentDate)
        {
            var clock = Substitute.For<IClock>();
            clock.GetCurrentDate().Returns(currentDate);
            return clock;
        }

        private static SearchRequest<UnusualNominalCodesParameters> CreateSearchRequest()
        {
            return new SearchRequest<UnusualNominalCodesParameters>(new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2), new DateRange(DateTime.MinValue, DateTime.MaxValue)),1);
        }
    }
}
