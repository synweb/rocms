using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Contract.Models
{
    public class TagStat
    {
        public int Mentions { get; set; }
        public int TagId { get; set; }
        public string Name { get; set; }
    }
}
