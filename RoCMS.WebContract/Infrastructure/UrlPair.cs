namespace RoCMS.Web.Contract.Infrastructure
{
    public class UrlPair
    {
        public UrlPair(string relativeUrl, string canonicalUrl)
        {
            RelativeUrl = relativeUrl;
            CanonicalUrl = canonicalUrl;
        }

        public UrlPair()
        {
            
        }

        public string RelativeUrl { get; set; }
        public string CanonicalUrl { get; set; }
    }
}
