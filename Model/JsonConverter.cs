using System;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace Model
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
                return JsonConvert.DeserializeObject<T>((string) value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}