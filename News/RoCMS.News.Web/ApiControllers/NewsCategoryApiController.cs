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
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.News)]
    public class NewsCategoryApiController : ApiController
    {
        private readonly INewsCategoryService _categoryService;
        private readonly ILogService _logService;

        public NewsCategoryApiController(INewsCategoryService categoryService, ILogService logService)
        {
            _categoryService = categoryService;
            _logService = logService;
        }

        [HttpPost]
        public ResultModel Create(Category category)
        {
            try
            {
                int id = _categoryService.CreateCategory(category);
                return new ResultModel(true, new { id = id });
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Update(Category category)
        {
            try
            {
                _categoryService.UpdateCategory(category);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Remove(int categoryId)
        {
            try
            {

                _categoryService.DeleteCategory(categoryId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpGet]
        public ICollection<Category> GetCategories()
        {
            return _categoryService.GetCategories();
        }

        [HttpPost]
        public ResultModel UpdateSortOrder(ICollection<Category> categories)
        {
            try
            {
                _categoryService.UpdateCategoriesSortOrder(categories);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}