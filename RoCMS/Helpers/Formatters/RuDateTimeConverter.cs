using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RoCMS.Helpers.Formatters
{
    public class RuDateTimeConverter : DateTimeConverterBase
    {
        private readonly IsoDateTimeConverter _isoDateTimeConverter = new IsoDateTimeConverter();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                DateTime result;
                bool succeed = DateTime.TryParseExact(reader.Value.ToString(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out result);
                if (!succeed)
                {
                    result = DateTime.ParseExact(reader.Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                return result;
            }
            catch (Exception e)
            {
                return _isoDateTimeConverter.ReadJson(reader, objectType, existingValue, serializer);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            _isoDateTimeConverter.WriteJson(writer, value,serializer);
            //writer.WriteValue(((DateTime)value).ToString("dd.MM.yyyy HH:mm"));
        }
    }
}