using System;
using Audition.Chromium;
using Autofac;
using ExcelImport;
using Model.Time;
using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;
using Webapp;
using Webapp.Requests;

namespace Tests
{
    [TestFixture]
    public class JsonConverterTests
    {        
        [Test]
        public void CanDeserializeAccountsSearchWindow()
        {
            var result = Parse<SearchWindow<UnusualNominalCodesParameters>>(@"{
            Period: {
                From: '2012-4-5',
                To: '2013-4-4'
            },

            Parameters: {
                MinimumEntriesToBeConsideredNormal: 2
            }
        }");

            Assert.AreEqual(new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(2012, 4, 5), new DateTime(2013, 4, 4))), 
                result);
        } 
        
        [Test]
        public void CanDeserializeSearchRequest()
        {
            var result = Parse<SearchRequest<UnusualNominalCodesParameters>>(@"{
            pageNumber: 7,
            searchWindow: {
                Period: {
                    From: '2012-4-5',
                    To: '2013-4-4'
                },

                Parameters: {
                    MinimumEntriesToBeConsideredNormal: 2
                }
        }}");

            Assert.AreEqual(new SearchRequest<UnusualNominalCodesParameters>(new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(2012, 4, 5), new DateTime(2013, 4, 4))), 7), 
                result);
        }

        [Test]
        public void CanDeserializeImportMapping()
        {
            var result = Parse<ExcelImportMapping>(@"{
    'SheetDescription':{
        'Filename':'C:\\code\\Audition\\build\\Debug\\Dev\\ExampleSage50Export.xlsx',
        'UseHeaderRow':true,
        'Sheet':2
    },
    'Lookups':{
        'TransactionDate':2,
        'Description':4,
        'Username':8,
        'NominalCode':16,
        'NominalName':32,
        'Amount':64,
        'Id':128,
        'Type':256}
}");
            var fieldLookups = new FieldLookups(transactionDate: 2,
                description: 4,
                username: 8,
                nominalCode: 16,
                nominalName: 32,
                amount: 64,
                id: 128,
                type: 256);

            Assert.AreEqual(fieldLookups,result.Lookups);
            var sheetDescription = result.SheetDescription;
            Assert.AreEqual(true,sheetDescription.UseHeaderRow);
            Assert.AreEqual(2,result.SheetDescription.Sheet);
            Assert.AreEqual(@"C:\code\Audition\build\Debug\Dev\ExampleSage50Export.xlsx",sheetDescription.Filename);
        }

       [Test]
       public void CanDeserializePeriod()
       {           
           var result = Parse<DateRange>(@"{
               From: '2012-4-5',
               To: '2013-4-4'
           }");
           Assert.AreEqual(new DateRange(new DateTime(2012,4,5),new DateTime(2013, 4, 4) ), result);
       }

        private static T Parse<T>(string value)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<HttpModule>();
            using (var scope = builder.Build())
            {
                var settings = scope.Resolve<JsonSerializerSettings>();
                return JsonConvert.DeserializeObject<T>(value, settings);
            }
        }
    }
}
