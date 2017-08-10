using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.News.Contract.Models;

namespace RoCMS.News.Contract.Services
{
    public interface IBlogService
    {
        
        Blog GetBlog(int blogId);
        Blog GetBlog(string relativeUrl);
        int CreateBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void AttachUser(int blogId, int userId);
        void DetachUser(int blogId, int userId);
        IList<Blog> GetBlogs();
        Blog GetUserBlog(int userId);
        bool CheckIfExists(string relativeUrl);
        bool CheckIfUserHasAccess(int userId, int blogId);
        void UpdateBlogByClient(Blog blog);
        void DeleteBlog(int blogId);
    }
}
