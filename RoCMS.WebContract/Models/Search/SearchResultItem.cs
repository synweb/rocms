namespace RoCMS.Web.Contract.Models.Search
{
    public class SearchResultItem
    {
        public string SearchItemKey { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageId { get; set; }
        public int Relevance { get; set; }

    }
}
