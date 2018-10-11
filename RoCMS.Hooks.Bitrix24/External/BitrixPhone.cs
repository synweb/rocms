using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Hooks.Bitrix24.External
{
    class BitrixContact
    {
        [JsonProperty("VALUE")]
        public string Value { get; set; }
        [JsonProperty("VALUE_TYPE")]
        public string ValueType { get; set; }
    }
}
