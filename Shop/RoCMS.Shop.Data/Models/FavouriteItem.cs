using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Shop.Data.Models
{
    public class FavouriteItem
    {
        public Guid SessionId { get; set; }
        public int HeartId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
