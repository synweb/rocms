using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract
{
    public class Widget
    {
        public Widget(string pattern, string viewPath)
        {
            Pattern = pattern;
            ViewPath = viewPath;
        }

        public string Pattern { get; set; }
        public string ViewPath { get; set; }
    }
}
