namespace RoCMS.News.Contract.Models
{
    public class RssCrawlerFilter
    {
        public int RssCrawlerFilterId { get; set; }
        public int RssCrawlerId { get; set; }
        public string Filter { get; set; }
    }
}
