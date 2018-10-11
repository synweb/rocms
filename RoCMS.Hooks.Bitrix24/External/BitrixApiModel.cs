using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Hooks.Bitrix24.External
{
    class BitrixApiModel
    {
        [JsonProperty("FIELDS")]
        public object Fields { get; set; }
        [JsonProperty("PARAMS")]
        public object Params { get; set; }
    }
}
