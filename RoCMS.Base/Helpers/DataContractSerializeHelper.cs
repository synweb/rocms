using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace RoCMS.Base.Helpers
{
    public static class DataContractSerializeHelper
    {
        public static string SerializeToXmlString<T>(T data) where T : class
        {
            if (data == null)
                return null;

            var dataContractSerializer = new DataContractSerializer(typeof(T));
            var stringBuilder = new StringBuilder();
            using (var sw = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    dataContractSerializer.WriteObject(writer, data);
                }
            }
            return stringBuilder.ToString();
        }

        public static byte[] SerializeToBytes<T>(T data) where T : class
        {
            if (data == null)
                return null;

            using (var stream = new MemoryStream())
            {
                var dataContractSerializer = new DataContractSerializer(typeof(T));
                dataContractSerializer.WriteObject(stream, data);
                return stream.ToArray();
            }
        }

        public static T Deserialize<T>(string xmlData) where T : class, new()
        {
            if (xmlData == null)
                return null;

            T result = null;
            if (!String.IsNullOrEmpty(xmlData))
            {
                using (var srd = new StringReader(xmlData))
                {
                    using (var reader = new XmlTextReader(srd))
                    {
                        var sr = new DataContractSerializer(typeof(T));
                        try
                        {
                            result = (T)sr.ReadObject(reader);
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e);
                        }
                    }
                }
            }
            else
            {
                result = new T();
            }

            return result;
        }

        public static T Deserialize<T>(byte[] data) where T : class, new()
        {
            if (data == null) return null;
            using (var stream = new MemoryStream(data))
            {
                var serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        public static Dictionary<string, string> DeserializeJson(string jsonData)
        {
            if (String.IsNullOrEmpty(jsonData)) return null;
            var res = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
            return res;
        }

        public static string SerializeJson(Dictionary<string, string> data)
        {
            if (data == null) return null;
            var res = JsonConvert.SerializeObject(data);
            return res;
        }
    }
}
