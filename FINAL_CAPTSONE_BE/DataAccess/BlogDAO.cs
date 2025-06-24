using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BlogDAO
    {
        private readonly WeddingWonderDbContext context;

        public BlogDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Blog>> GetBlogs()
        {
            try
            {
                return await context.Blogs.Include(b => b.Tags).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Blog> GetBlogById(int blogId)
        {
            try
            {
                return await context.Blogs.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateBlog(Blog blog)
        {
            try
            {
                await context.Blogs.AddAsync(blog);
                await context.SaveChangesAsync();  
                return true;  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public Task UpdateBlog(int id, Blog blog)
        {
            try
            {
                context.Blogs.Update(blog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return Task.CompletedTask;
        }

        public async Task DeleteBlog(int blogId)
        {
            try
            {
                var blogToDelete = await context.Blogs.FindAsync(blogId);
                if (blogToDelete != null)
                {
                    context.Blogs.Remove(blogToDelete);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Blog not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Blog>> GetBlogsByTag(string tag)
        {
            try
            {
                return await context.Blogs
                    .Include(b => b.Tags) 
                    .Where(b => b.Tags.Any(t => t.Name == tag))  
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
