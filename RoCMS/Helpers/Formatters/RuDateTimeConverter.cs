using System;
using System.Globalization;
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
                // приоритетным делаем российский формат даты-вермени
                bool succeed = DateTime.TryParseExact(reader.Value.ToString(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out result);
                if (!succeed)
                {
                    // если времени нет, парсим просто дату
                    result = DateTime.ParseExact(reader.Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                return result;
            }
            catch (Exception e)
            {
                // если ничего не вышло, пробуем парсить как обычно
                return _isoDateTimeConverter.ReadJson(reader, objectType, existingValue, serializer);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            _isoDateTimeConverter.WriteJson(writer, value,serializer);
        }
    }
}