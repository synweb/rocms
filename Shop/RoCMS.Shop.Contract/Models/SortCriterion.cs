namespace RoCMS.Shop.Contract.Models
{
    public enum SortCriterion
    {
        Relevance,
        Article,
        CreationDateDesc,
        CreationDateAsc,
        //Мы пока не умеем сортировать по релевантности
        //Relevance,
        PriceAsc,
        PriceDesc,

        RatingAsc,
        RatingDesc
        
    }
}
