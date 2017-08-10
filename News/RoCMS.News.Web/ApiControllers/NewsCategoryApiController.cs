using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.News)]
    public class NewsCategoryApiController : ApiController
    {
        private readonly INewsCategoryService _categoryService;

        public NewsCategoryApiController(INewsCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public ResultModel Create(Category category)
        {
            int id = _categoryService.CreateCategory(category);
            return new ResultModel(true, new { id = id });
        }

        [HttpPost]
        public ResultModel Update(Category category)
        {
            _categoryService.UpdateCategory(category);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Remove(int categoryId)
        {
            _categoryService.DeleteCategory(categoryId);
            return ResultModel.Success;
        }

        [HttpGet]
        public ICollection<Category> GetCategories()
        {
            return _categoryService.GetCategories();
        }

        [HttpPost]
        public ResultModel UpdateSortOrder(ICollection<Category> categories)
        {
            _categoryService.UpdateCategoriesSortOrder(categories);
            return ResultModel.Success;
        }
    }
}