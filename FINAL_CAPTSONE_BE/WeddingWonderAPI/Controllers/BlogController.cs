using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogServiceManager;

        public BlogController(BlogService blogServiceManager)
        {
            _blogServiceManager = blogServiceManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetBlogs()
        {
            try
            {
                List<BlogDTO> blogs = await _blogServiceManager.GetAllBlogsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved blog information successfully",
                    Data = blogs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBlogById(int id)
        {
            try
            {
                BlogDTO? blog = await _blogServiceManager.GetBlogByIdAsync(id);
                if (blog == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Blog not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved blog information successfully",
                    Data = blog
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("tag/{tag}")]
        public async Task<ActionResult> GetBlogsByTag(string tag)
        {
            try
            {
                List<BlogDTO>? blogs = await _blogServiceManager.GetBlogsByTagAsync(tag);
                if (blogs == null || !blogs.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No blogs found with the given tag"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved blogs by tag successfully",
                    Data = blogs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost("create")]
       
        public async Task<IActionResult> CreateBlog([FromBody] BlogDTO blogDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data"
                });
            }

            try
            {
                await _blogServiceManager.CreateBlogAsync(blogDto);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Blog created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPut("update/{id}")]
       
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogDTO blogDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data"
                });
            }

            try
            {
                bool success = await _blogServiceManager.UpdateBlogAsync(id, blogDto);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Blog not found or update failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Blog updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpDelete("delete/{id}")]
        
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                bool success = await _blogServiceManager.DeleteBlogAsync(id);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Blog not found or deletion failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Blog deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
    }
}
