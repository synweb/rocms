using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Hooks.Bitrix24.External
{
    class BitrixLead
    {
        [JsonProperty("TITLE")]
        public string Title { get; set; }
        [JsonProperty("NAME")]
        public string Name { get; set; }
        [JsonProperty("SECOND_NAME")]
        public string SecondName { get; set; }
        [JsonProperty("LAST_NAME")]
        public string LastName { get; set; }
        [JsonProperty("STATUS_ID")]
        public string StatusId { get; set; }
        [JsonProperty("OPENED")]
        public string Opened { get; set; }
        [JsonProperty("COMMENTS")]
        public string Comments { get; set; }
        [JsonProperty("ASSIGNED_BY_ID")]
        public int AssignedById { get; set; }
        [JsonProperty("CURRENCY_ID")]
        public string CurrencyId { get; set; }
        [JsonProperty("OPPORTUNITY")]
        public int Opportunity { get; set; }
        [JsonProperty("PHONE")]
        public BitrixContact[] Phone { get; set; }
        [JsonProperty("EMAIL")]
        public BitrixContact[] Email { get; set; }
    }
}
