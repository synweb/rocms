using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
    public class CategoryApiController : ApiController
    {
        private readonly IShopService _shopService;

        public CategoryApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpPost]
        public ResultModel Create(Category category)
        {
            int id = _shopService.CreateCategory(category);
            return new ResultModel(true, new {id = id});
        }

        [HttpPost]
        public ResultModel Update(Category category)
        {
            _shopService.UpdateCategory(category);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Remove(int categoryId)
        {
            _shopService.DeleteCategory(categoryId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<Category> GetCategories()
        {
            return _shopService.GetCategories();
        }

        [HttpPost]
        public ResultModel UpdateSortOrder(IList<Category> categories)
        {
            _shopService.UpdateCategoriesSortOrder(categories);
            return ResultModel.Success;
        }
    }
}
