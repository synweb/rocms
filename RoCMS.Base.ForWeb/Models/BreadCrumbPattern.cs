using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.ForWeb.Models
{
    public class BreadCrumbPattern
    {
        public BreadCrumbPattern(string regex, BreadCrumbHandler handler)
        {
            Regex = regex;
            Handler = handler;
        }

        public string Regex { get; private set; }
        public BreadCrumbHandler Handler { get; private set; }
    }

    public delegate IList<BreadCrumb> BreadCrumbHandler(string url);
}
