using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
    public class CategoryApiController : ApiController
    {
        private readonly IShopCategoryService _shopCategoryService;

        public CategoryApiController(IShopCategoryService shopCategoryService)
        {
            _shopCategoryService = shopCategoryService;
        }

        [HttpPost]
        public ResultModel Create(Category category)
        {
            int id = _shopCategoryService.CreateCategory(category);
            return new ResultModel(true, new { id = id });
        }

        [HttpPost]
        public ResultModel Update(Category category)
        {
            _shopCategoryService.UpdateCategory(category);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Remove(int categoryId)
        {
            _shopCategoryService.DeleteCategory(categoryId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<Category> GetCategories()
        {
            return _shopCategoryService.GetCategories();
        }

        [HttpPost]
        public ResultModel UpdateSortOrder(IList<Category> categories)
        {
            _shopCategoryService.UpdateCategoriesSortOrder(categories);
            return ResultModel.Success;
        }

        public IList<CategoryWithStats> GetCategoriesWithStats(int? parentId)
        {
            var allCats = _shopCategoryService.GetAllCategories();
            var allStats = _shopCategoryService.GetCategoryStats();

            var stats = parentId.HasValue ? allStats.Where(x => _shopCategoryService.IsChild(x.Key, parentId.Value)) : allStats;

            var result = new List<CategoryWithStats>();

            foreach (var st in stats)
            {
                result.Add(new CategoryWithStats() { Category = allCats.Single(x => x.HeartId == st.Key), Count = st.Value});
            }
            return result;
        }

        public class CategoryWithStats
        {
            public Category Category { get; set; }
            public int Count { get; set; }
        }
    }
}
