using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Audition;
using Model;
using NodaTime;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class JsonConverterTests
    {
        [Test]
        public void CanDeserializeSearchWindow()
        {
            var result = Parse<SearchWindow>(@"{
            Period: {
                From: '2012-4-5',
                To: '2013-4-4'
            },

            Outside: {
                FromDay: 'Monday',
                ToDay: 'Friday',
                FromTime: '08:00:00',
                ToTime: '18:00:00'
            }
        }");      
      
            Assert.AreEqual(new SearchWindow(new TimeFrame(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8,0), new LocalTime(18,0)),
                new DateRange(new DateTime(2012,4,5),new DateTime(2013,4,4) ) ), 
                result);
        }

        
       [Test]
       public void CanDeserializeTimeFrame()
       {           
           var result = Parse<TimeFrame>(@"{
               FromDay: 'Monday',
               ToDay: 'Friday',
               FromTime: '08:00:00',
               ToTime: '18:00:00'
           }");
           Assert.AreEqual(new TimeFrame(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(8,0),new LocalTime(18,0)), result);
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
            var parser = new JsonModelBinder();
            var modelBindingContext = GetBindingContext(value, typeof(T));
            var httpActionContext = GetJsonRequest();

            parser.BindModel(httpActionContext, modelBindingContext);
            return (T) modelBindingContext.Model;
        }

        private static ModelBindingContext GetBindingContext(string value, Type type)
        {            
            var modelBindingContext = new ModelBindingContext();
            modelBindingContext.ModelMetadata = new ModelMetadata(new EmptyModelMetadataProvider(), type, null, type, "");
            var valueProvider = Substitute.For<IValueProvider>();
            valueProvider.GetValue(Arg.Any<string>()).Returns(new ValueProviderResult(value, "steve", CultureInfo.CurrentCulture));

            modelBindingContext.ValueProvider = valueProvider;                     

            return modelBindingContext;
        }

        private static HttpActionContext GetJsonRequest()
        {
            return new HttpActionContext(new HttpControllerContext(new HttpRequestContext(), new HttpRequestMessage(){Content = new StringContent("", Encoding.UTF8, "application/json")}, new HttpControllerDescriptor(), Substitute.For<IHttpController>()),new ReflectedHttpActionDescriptor() );
        }       
    }
}
