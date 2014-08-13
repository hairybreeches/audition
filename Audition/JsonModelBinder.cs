using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using NodaTime.Serialization.JsonNet;
using NodaTime.TimeZones;

namespace Audition
{
    public class JsonModelBinder : IModelBinder
    {        

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (IsJsonRequest(actionContext))
            {
                var value = GetStringValue(bindingContext);
                bindingContext.Model = ParseJson(bindingContext, value);
                return true;
            }
            return false;            
        }

        private static bool IsJsonRequest(HttpActionContext actionContext)
        {
            return String.Equals("application/json", actionContext.Request.Content.Headers.ContentType.MediaType, StringComparison.InvariantCultureIgnoreCase);
        }

        private static object ParseJson(ModelBindingContext bindingContext, string value)
        {
            var settings = new JsonSerializerSettings();
            settings.ConfigureForNodaTime(new DateTimeZoneCache(new BclDateTimeZoneSource()));            
            return JsonConvert.DeserializeObject(value, bindingContext.ModelType, settings);
        }

        private static string GetStringValue(ModelBindingContext bindingContext)
        {
            var val = bindingContext.ValueProvider.GetValue(
                bindingContext.ModelName);
            return val.RawValue as string;            
        }
    }
}