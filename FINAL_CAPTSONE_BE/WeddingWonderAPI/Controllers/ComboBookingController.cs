using BusinessObject.DTOs;
using BusinessObject.Requests;
using BusinessObject.Requests.ComboBooking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/comboBooking")]
    [ApiController]
    public class ComboBookingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ComboBookingService _comboService;

        public ComboBookingController(IConfiguration configuration, ComboBookingService comboService)
        {
            _configuration = configuration;
            _comboService = comboService;
        }

        //Creat
        [HttpPost("createFullService")]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] ComboBookingDTO request)
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
                await _comboService.CreateBookingFull(userId, request);

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

        [HttpPost("createPartialService")]
        [Authorize]
        public async Task<ActionResult> PartialService([FromBody] ComboBookingDTO request)
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
                await _comboService.CreateBookingPartial(userId, request);

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
                ComboBookingDTO bookingDTO = await _comboService.GetById(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get single booking successfully",
                    Data = bookingDTO
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
        [HttpGet("getRequestPayment")]
        [Authorize]
        public async Task<ActionResult> GetRequestPayment()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                List<RequestPayment> requestPayment = await _comboService.GetRequestPayment(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get request payment successfully",
                    Data = requestPayment
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
        public async Task<ActionResult> UpdateBooking(int id, [FromBody] ComboBookingDTO request)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _comboService.CheckCustomerAndBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified customer."
                    });

                await _comboService.UpdateBooking(id, request);
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

        [HttpPut("supplierUpdate/detailId={id}/type={type}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> SupplierUpdateBooking(int id, int type, [FromBody] SupplierUpdate request)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _comboService.CheckSupplierAndDetailBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _comboService.SupplierUpdateBooking(id, type, request);
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

        [HttpPut("reselectRejected/bookingId={bookingId}/typeId={typeId}")]
        [Authorize]
        public async Task<ActionResult> ReselectRejected(int bookingId, int typeId, [FromBody] ReselectRejected request)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _comboService.CheckCustomerAndBooking(userId, bookingId))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified customer."
                    });

                await _comboService.ReselectRejected(bookingId, request, typeId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Reselect booking successfully",
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
                if (!await _comboService.CheckSupplierAndDetailBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _comboService.AcceptBooking(id);
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
                if (!await _comboService.CheckSupplierAndDetailBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _comboService.RejectBooking(id);
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

        [HttpPut("cancelAll/{id}")]
        [Authorize]
        public async Task<ActionResult> CancelAllBooking(int id)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _comboService.CheckCustomerAndBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified user."
                    });

                await _comboService.CancelAllBooking(id);
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

        [HttpPut("cancel/{id}")]
        [Authorize]
        public async Task<ActionResult> CancelBooking(int id)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _comboService.CheckCustomerAndDetailBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified user."
                    });

                await _comboService.CancelBooking(id);
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
                if (!await _comboService.CheckSupplierAndDetailBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _comboService.UsingBooking(id);
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
                if (!await _comboService.CheckSupplierAndDetailBooking(supplierId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified supplier."
                    });

                await _comboService.FinishBooking(id);
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
                if (!await _comboService.CheckCustomerAndDetailBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified user."
                    });

                await _comboService.ConfirmFinishBooking(id);
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

        [HttpPut("confirmFinishAll/{id}")]
        [Authorize]
        public async Task<ActionResult> ConfirmFinishCombo(int id)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                if (!await _comboService.CheckCustomerAndBooking(userId, id))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = $"The booking does not belong to the specified user."
                    });

                await _comboService.ConfirmFinishBooking(id);
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

        //Delete
    }
}
