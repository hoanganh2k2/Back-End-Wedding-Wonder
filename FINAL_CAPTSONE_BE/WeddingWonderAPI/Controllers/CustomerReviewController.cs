using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class CustomerReviewController : ControllerBase
    {
        private readonly ICustomerReviewRepository _reviewRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ReviewService _reviewService;
        private readonly IConfiguration _configuration;

        public CustomerReviewController(IConfiguration configuration, ReviewService reviewService, ICustomerReviewRepository reviewRepository, IServiceRepository serviceRepository)
        {
            _configuration = configuration;
            _reviewService = reviewService;
            _reviewRepository = reviewRepository;
            _serviceRepository = serviceRepository;
        }

        [HttpGet("getAllReviews")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<ActionResult> GetAllReview()
        {
            try
            {
                List<CustomerReviewDTO> reviews = await _reviewService.GetAllReviews();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list reviews successfully",
                    Data = reviews
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("getReviewsOfStore/{id}")]
        public async Task<ActionResult> GetReviews(int id)
        {
            try
            {
                List<CustomerReviewDTO> reviews = await _reviewService.GetReviewsOfStore(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list reviews successfully",
                    Data = reviews
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("getAllReviewsForSupplier")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetReviewsForSupplier()
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                List<CustomerReviewDTO> reviews = await _reviewService.GetReviewsForSupplier(supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list reviews successfully",
                    Data = reviews
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetReview(int id)
        {
            try
            {
                CustomerReviewDTO review = await _reviewService.GetReviewById(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get review successfully",
                    Data = review
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("getHistory")]
        [Authorize]
        public async Task<ActionResult> GetHistory()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                List<CustomerReviewDTO> review = await _reviewService.GetHistory(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get history review successfully",
                    Data = review
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] CustomerReviewDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }
            try
            {
                int userId = UserHelper.GetUserId(User);
                request.UserId = userId;
                bool status = await _reviewService.CreateReview(request);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Review created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to create review"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpDelete("userDelete/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                await _reviewService.UserDeleteReview(id, userId);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Review has been delete"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpDelete("adminDelete/{id}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<ActionResult> AdminDelete(int id)
        {
            try
            {
                await _reviewService.AdminDeleteReview(id);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Review has been delete"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] CustomerReviewDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }

            if (id != request.ReviewId)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "ID in the URL does not match ID in the request body.",
                });
            }

            try
            {
                int userId = UserHelper.GetUserId(User);
                await _reviewService.UpdateReview(id, request, userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"You have successfully edited the review."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPut("reply/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> Reply(int id, [FromBody] string reply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }

            try
            {
                int supplierId = UserHelper.GetUserId(User);
                await _reviewService.Reply(id, reply, supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"You have responded successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPut("editReply/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> EditReply(int id, [FromBody] string reply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }

            try
            {
                int supplierId = UserHelper.GetUserId(User);
                await _reviewService.Reply(id, reply, supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"You have edited respond successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPut("deleteReply/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> DeleteReply(int id)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                await _reviewService.Reply(id, null, supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"You have deleted respond successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
