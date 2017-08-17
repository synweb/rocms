using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace RoCMS.Helpers.Formatters
{
    public class JsonCamelCaseFormatter: MediaTypeFormatter
    {
        private Encoding encoding;

       private JsonSerializerSettings _jsonSerializerSettings;

       public JsonCamelCaseFormatter()
       {
           _jsonSerializerSettings = new JsonSerializerSettings
           {
               ContractResolver = new CamelCasePropertyNamesContractResolver()
           };
           //_jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());
           _jsonSerializerSettings.Converters.Add(new RuDateTimeConverter());
           _jsonSerializerSettings.Converters.Add(new StringEnumConverter());

           // Fill out the mediatype and encoding we support
           SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
           encoding = new UTF8Encoding(false, true);
       }
    
        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content, IFormatterLogger formatterContext)
       {
           // Create a serializer
           JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

           // Create task reading the content
           return Task.Factory.StartNew(() =>
           {
               using (StreamReader streamReader = new StreamReader(stream, encoding))
               {
                   using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                   {
                       return serializer.Deserialize(jsonTextReader, type);
                   }
               }
           });
       }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
       {
           // Create a serializer
           JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);
    
           // Create task writing the serialized content
           return Task.Factory.StartNew(() =>
           {
               using (JsonTextWriter jsonTextWriter = new JsonTextWriter(new StreamWriter(stream, encoding)) { CloseOutput = false })
               {
                   serializer.Serialize(jsonTextWriter, value);
                   jsonTextWriter.Flush();
               }
           });
       }
   
    }
}