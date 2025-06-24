using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDAO _dao;

        public BlogRepository(BlogDAO dao)
        {
            _dao = dao;
        }

        public async Task<bool> CreateAsync(Blog blog)
        {
            return await _dao.CreateBlog(blog);
        }

        public async Task DeleteAsync(int id)
        {
            await _dao.DeleteBlog(id);
        }

        public async Task<Blog> GetAsyncById(int id)
        {
            return await _dao.GetBlogById(id);
        }

        public async Task<List<Blog>> GetsAsync()
        {
            return await _dao.GetBlogs();
        }

        public async Task UpdateAsync(int id, Blog blog)
        {
            await _dao.UpdateBlog(id, blog);
        }
        public async Task<List<Blog>> GetBlogByTagAsync(string tag)
        {
            return await _dao.GetBlogsByTag(tag);
        }
    }
}
