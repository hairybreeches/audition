using System;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;
using NodaTime.Serialization.JsonNet;
using NodaTime.TimeZones;

namespace Audition
{
    public class JsonConverter<T> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {            

            
            if (value is string)
            {
                var settings = new JsonSerializerSettings();
                settings.ConfigureForNodaTime(new DateTimeZoneCache(new BclDateTimeZoneSource()));
                return JsonConvert.DeserializeObject<T>((string) value, settings);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}