using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoCMS.Models
{
    public class BreadCrumb
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsLast { get; set; }
    }
}