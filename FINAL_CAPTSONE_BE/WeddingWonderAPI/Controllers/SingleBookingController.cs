using BusinessObject.DTOs;
using BusinessObject.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/singleBooking")]
    [ApiController]
    public class SingleBookingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SingleBookingService _singleService;

        public SingleBookingController(IConfiguration configuration, SingleBookingService singleService)
        {
            _configuration = configuration;
            _singleService = singleService;
        }

        //Creat
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] SingleBookingDTO request)
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
                await _singleService.CreateBooking(userId, request);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Booking has been successfully created"
                });
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

        //Read
        [HttpGet("getId/{id}")]
        [Authorize]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                SingleBookingToShow bookingToShows = await _singleService.GetById(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get single booking successfully",
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

        [HttpGet("getsAllOfSupplier")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsAllOfSupplier()
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<SingleBookingToShow> bookingToShows = await _singleService.GetsAllOfSupplierId(supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list singlebooking successfully",
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

        [HttpGet("getsAllOfUser")]
        [Authorize]
        public async Task<ActionResult> GetsAllOfUser()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);

                List<SingleBookingToShow> bookingToShows = await _singleService.GetsAllOfUser(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list singlebooking successfully",
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

        [HttpGet("getsAccept")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsAcceptOfService()
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<SingleBookingToShow> bookingToShows = await _singleService.GetsAcceptOfSupplier(supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list bookign accept successfully",
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

        [HttpGet("getsReject")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsRejectOfService()
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<SingleBookingToShow> bookingToShows = await _singleService.GetsRejectOfSupplier(supplierId);
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

        [HttpGet("getsCancel")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsCancelOfService()
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<SingleBookingToShow> bookingToShows = await _singleService.GetsCancelOfSupplier(supplierId);
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

        [HttpGet("getsRequest")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetsRequestBookings()
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);

                List<SingleBookingToShow> bookingToShows = await _singleService.GetsRequestBooking(supplierId);
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

        [HttpGet("countSuccess/supplierId/{id}")]
        [Authorize(Roles = "Supplier, Admin, Super Admin")]
        public async Task<ActionResult> CountSuccessSupplierId(int id)
        {
            try
            {

                int count = await _singleService.GetCountStatusBySupplierId(id, 6);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get the number of bookings successted successfully",
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

        [HttpGet("countReject/supplierId/{id}")]
        [Authorize(Roles = "Supplier, Admin, Super Admin")]
        public async Task<ActionResult> CountRejectSupplierId(int id)
        {
            try
            {

                int count = await _singleService.GetCountStatusBySupplierId(id, 4);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get the number of bookings rejected successfully",
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

        [HttpGet("countSuccess/serviceId/{id}")]
        [Authorize(Roles = "Supplier, Admin, Super Admin")]
        public async Task<ActionResult> CountSuccessServiceId(int id)
        {
            try
            {

                int count = await _singleService.GetCountStatusByServiceId(id, 6);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get the number of bookings successted successfully",
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

        [HttpGet("countReject/serviceId/{id}")]
        [Authorize(Roles = "Supplier, Admin, Super Admin")]
        public async Task<ActionResult> CountRejectServiceId(int id)
        {
            try
            {

                int count = await _singleService.GetCountStatusByServiceId(id, 4);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get the number of bookings rejected successfully",
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

        [HttpGet("countSuccess/userId/{id}")]
        [Authorize]
        public async Task<ActionResult> CountSuccessUserId(int id)
        {
            try
            {

                int count = await _singleService.GetCountStatusByUserId(id, 6);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get the number of bookings successted successfully",
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

        [HttpGet("countCancel/userId/{id}")]
        [Authorize]
        public async Task<ActionResult> CountCancelUserId(int id)
        {
            try
            {

                int count = await _singleService.GetCountStatusByUserId(id, 0);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get the number of bookings canceled successfully",
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

        //Update
        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateBooking(int id, [FromBody] SingleBookingDTO request)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckCustomerAndBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified customer."
                    });

                await _singleService.UpdateBooking(id, request);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Update booking successfully",
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

        [HttpPut("updateForSup/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> UpdateBookingForSup(int id, [FromBody] SingleBookingDTO request)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckSupplierAndBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _singleService.UpdateBookingForSup(id, request);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Update booking successfully",
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

        [HttpPut("accept/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> AcceptBooking(int id)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckSupplierAndBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _singleService.AcceptBooking(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Accept booking successfully",
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

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> RejectBooking(int id)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckSupplierAndBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _singleService.RejectBooking(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Reject booking successfully",
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

        [HttpPut("cancel/{id}")]
        [Authorize]
        public async Task<ActionResult> CancelBooking(int id)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckCustomerAndBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified user."
                    });

                await _singleService.CancelBooking(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Cancel booking successfully",
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

        [HttpPut("using/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> UsingBooking(int id)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckSupplierAndBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _singleService.UsingBooking(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Using booking successfully",
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

        [HttpPut("finish/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> FinishBooking(int id)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckSupplierAndBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _singleService.FinishBooking(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Finish booking successfully",
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

        [HttpPut("confirmFinish/{id}")]
        [Authorize]
        public async Task<ActionResult> ConfirmFinishBooking(int id)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _singleService.CheckCustomerAndBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified user."
                    });

                await _singleService.ConFirmFinishBooking(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"ConFirm finish booking successfully",
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
        //Delete
    }
}
