using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models
{
    /// <summary>
    /// Класс используется для маппинга... В будущем мб стоит перенести его в контракт
    /// </summary>
    [DataContract]
    public class Address
    {
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string House { get; set; }
        [DataMember]
        public string Metro { get; set; }
        [DataMember]
        public string FrontNumber { get; set; }
        [DataMember]
        public string HouseIndex { get; set; }
        [DataMember]
        public string Appartment { get; set; }
        [DataMember]
        public string Floor { get; set; }
        [DataMember]
        public string Intercom { get; set; }
    }
}
