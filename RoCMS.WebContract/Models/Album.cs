using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? OwnerId { get; set; }
        public DateTime CreationDate { get; set; }
        public int ImageCount { get; set; }
        public string WatermarkImageId { get; set; }
    }
}
