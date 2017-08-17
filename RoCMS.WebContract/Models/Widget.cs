namespace RoCMS.Web.Contract.Models
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
