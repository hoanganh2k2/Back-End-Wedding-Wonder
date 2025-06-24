using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess;
using Google;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;

namespace Services
{
    public class BlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly WeddingWonderDbContext _context;
        public BlogService(
            IBlogRepository blogRepository,
            IUnitOfWork unitOfWork , WeddingWonderDbContext context)
        {
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<List<BlogDTO>> GetAllBlogsAsync()
        {
            try
            {
                var blogs = await _context.Blogs
                    .Include(b => b.Tags) 
                    .ToListAsync(); 

                return blogs.Select(b => new BlogDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    CreateDate = b.CreateDate,
                    Image=b.Image,
                    Tags = b.Tags.Select(t => t.Name).ToList() 
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching blogs: {ex.Message}", ex);
            }
        }

        public async Task<BlogDTO?> GetBlogByIdAsync(int id)
        {
            try
            {
                var blog = await _context.Blogs
                    .Include(b => b.Tags)  
                    .FirstOrDefaultAsync(b => b.Id == id); 

                if (blog == null) return null;

                return new BlogDTO
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = blog.Content,
                    CreateDate = blog.CreateDate,
                    Image = blog.Image,
                    Tags = blog.Tags.Select(t => t.Name).ToList() 
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching blog by id: {ex.Message}", ex);
            }
        }

        public async Task<List<BlogDTO>> GetBlogsByTagAsync(string tag)
        {
            try
            {
                List<Blog> blogs = await _blogRepository.GetBlogByTagAsync(tag);
                return blogs.Select(b => new BlogDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    CreateDate = b.CreateDate,
                    Image = b.Image,
                    Tags = b.Tags.Select(t => t.Name).ToList() 
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateBlogAsync(BlogDTO blogDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var blog = new Blog
                {
                    Title = blogDto.Title,
                    Content = blogDto.Content,
                    CreateDate = DateTime.Now,
                    Image = blogDto.Image
                };
         
                foreach (var tagName in blogDto.Tags)
                {
                    var tag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name == tagName);

                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        await _context.Tags.AddAsync(tag);
                    }
                   
                    blog.Tags.Add(tag);
                }
  
                await _context.Blogs.AddAsync(blog);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateBlogAsync(int id, BlogDTO blogDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var blog = await _context.Blogs.Include(b => b.Tags)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (blog == null)
                    return false;

                blog.Title = blogDto.Title;
                blog.Content = blogDto.Content;
                blog.Image = blogDto.Image;
                blog.Tags.Clear();

                foreach (var tagName in blogDto.Tags)
                {
                    var tag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        await _context.Tags.AddAsync(tag);
                    }
                    blog.Tags.Add(tag);
                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _blogRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
