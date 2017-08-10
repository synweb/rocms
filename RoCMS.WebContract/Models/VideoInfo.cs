using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models
{
    public class VideoInfo
    {
        public DateTime CreationDate { get; set; }

        public string VideoId { get; set; }

        public string ImageId { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }

    }
}
