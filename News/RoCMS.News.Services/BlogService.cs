using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Data.Gateways;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Services
{
    public class BlogService:NewsService, IBlogService
    {
        private readonly BlogGateway _blogGateway = new BlogGateway();
        private readonly BlogUserGateway _blogUserGateway = new BlogUserGateway();
        private readonly ISearchService _searchService;

        public BlogService(ISearchService searchService)
        {
            _searchService = searchService;
            InitCache("NewsCategoryService");
        }

        public Blog GetBlog(int blogId)
        {
            var dataBlog = _blogGateway.SelectOne(blogId);
            if (dataBlog == null)
                return null;
            var result = Mapper.Map<Blog>(dataBlog);
            return result;
        }

        public Blog GetBlog(string relativeUrl)
        {
            var dataBlog = _blogGateway.SelectOneByRelativeUrl(relativeUrl);
            if (dataBlog == null)
                return null;
            var result = Mapper.Map<Blog>(dataBlog);
            return result;
        }

        public int CreateBlog(Blog blog)
        {
            var dataBlog = Mapper.Map<Data.Models.Blog>(blog);
            int id = _blogGateway.Insert(dataBlog);
            if (blog.OwnerId.HasValue)
            {
                AttachUser(id, blog.OwnerId.Value);
            }
            blog.BlogId = id;
            _searchService.UpdateIndex(blog);
            return id;
        }

        public void UpdateBlog(Blog blog)
        {
            var dataBlog = Mapper.Map<Data.Models.Blog>(blog);
            _blogGateway.Update(dataBlog);
            _searchService.UpdateIndex(blog);
        }

        public void AttachUser(int blogId, int userId)
        {
            _blogUserGateway.Insert(blogId, userId);
        }

        public void DetachUser(int blogId, int userId)
        {
            _blogUserGateway.Delete(blogId, userId);
        }

        public IList<Blog> GetBlogs()
        {
            var blogs = _blogGateway.Select();
            var result = Mapper.Map<IList<Blog>>(blogs);
            return result;
        }

        public Blog GetUserBlog(int userId)
        {
            int? blogId = _blogGateway.SelectByOwner(userId).FirstOrDefault()?.BlogId;
            if (blogId == null)
                return null;
            return GetBlog(blogId.Value);
        }

        public bool CheckIfExists(string relativeUrl)
        {
            //TODO: сделать одной хранимкой
            var blogs = _blogGateway.Select();
            var res = blogs.Any(x => relativeUrl.Equals(x.RelativeUrl));
            return res;
        }

        public bool CheckIfUserHasAccess(int userId, int blogId)
        {
            return _blogUserGateway.SelectByBlog(blogId).Any(x => x.UserId == userId);
        }

        public void UpdateBlogByClient(Blog blog)
        {
            var dataRec = Mapper.Map<Data.Models.Blog>(blog);
            var original = _blogGateway.SelectOne(blog.BlogId);
            {
                // владелец не меняется
                dataRec.OwnerId = original.OwnerId;
            }
            _blogGateway.Update(dataRec);
            _searchService.UpdateIndex(blog);
        }

        public void DeleteBlog(int blogId)
        {
            _blogGateway.Delete(blogId);
            _searchService.RemoveFromIndex(typeof(Blog), blogId);
        }
    }
}
