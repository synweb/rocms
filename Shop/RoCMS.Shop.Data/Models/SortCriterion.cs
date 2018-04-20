namespace RoCMS.Shop.Data.Models
{
    public enum SortCriterion
    {
        Relevance,
        Article,
        Random,
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
