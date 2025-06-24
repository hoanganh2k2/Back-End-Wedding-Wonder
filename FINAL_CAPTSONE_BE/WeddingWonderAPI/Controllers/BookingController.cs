using BusinessObject.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BookingService _bookingService;

        public BookingController(IConfiguration configuration, BookingService bookingService)
        {
            _configuration = configuration;
            _bookingService = bookingService;
        }

        //Read
        [HttpGet("getsAllOfUser")]
        [Authorize]
        public async Task<ActionResult> GetsAllOfUser()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsHistoryForCus(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking successfully",
                    Data = bookingToShows
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

        [HttpGet("getsAcceptCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsAcceptOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsAcceptOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking accept successfully",
                    Data = bookingToShows
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

        [HttpGet("getsFinishCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsFinishOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsFinishOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking finish successfully",
                    Data = bookingToShows
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

        [HttpGet("getsCancelCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsCancelOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsCancelOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking cancel successfully",
                    Data = bookingToShows
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

        [HttpGet("getsRequestCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsRequestOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsRequestOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking request successfully",
                    Data = bookingToShows
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

        [HttpGet("getsCountAcceptCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsCountAcceptOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountAcceptOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get count booking accept successfully",
                    Data = count
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

        [HttpGet("getsCountFinishCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsCountFinishOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountFinishOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get count booking finish successfully",
                    Data = count
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

        [HttpGet("getsCountCancelCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsCountCancelOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountCancelOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get count booking cancel successfully",
                    Data = count
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

        [HttpGet("getsCountRequestCus/{type:int}")]
        [Authorize]
        public async Task<ActionResult> GetsCountRequestOfCustomer(int type)
        {
            try
            {
                int customerId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountRequestOfCustomer(customerId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get count booking request successfully",
                    Data = count
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

        [HttpGet("getsAllOfSupplier")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsAllOfSupplier()
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsHistoryForSupplier(supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking successfully",
                    Data = bookingToShows
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

        [HttpGet("getsAcceptSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsAcceptOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsAcceptOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking accept successfully",
                    Data = bookingToShows
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

        [HttpGet("getsRejectSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsRejectOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsRejectOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking reject successfully",
                    Data = bookingToShows
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

        [HttpGet("getsCancelSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsCancelOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsCancelOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking cancel successfully",
                    Data = bookingToShows
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

        [HttpGet("getsRequestSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsRequestOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<BookingToShow> bookingToShows = await _bookingService.GetsRequestOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking request successfully",
                    Data = bookingToShows
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

        [HttpGet("getsCountAcceptSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsCountAcceptOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountAcceptOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking accept successfully",
                    Data = count
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

        [HttpGet("getsCountRejectSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsCountRejectOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountRejectOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking reject successfully",
                    Data = count
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

        [HttpGet("getsCountCancelSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsCountCancelOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountCancelOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking cancel successfully",
                    Data = count
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

        [HttpGet("getsCountRequestSup/{type:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsCountRequestOfSupplier(int type)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                int count = await _bookingService.GetsCountRequestOfSupplier(supplierId, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list booking request successfully",
                    Data = count
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
    }
}
