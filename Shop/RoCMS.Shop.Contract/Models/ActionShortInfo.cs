using System.Runtime.Serialization;
using RoCMS.Base.Models;

namespace RoCMS.Shop.Contract.Models
{
    [DataContract]
    public class ActionShortInfo: IdNamePair<int>
    {
        public int Discount { get; set; }
        public ActionShortInfo(int id, string name, int discount):base(id, name)
        {
            Discount = discount;
        }

        
    }
}
