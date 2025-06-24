using System.Collections.Specialized;
using System.Security.Claims;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Services;
using Services.Services;
using WeddingWonderAPI.Helper;

namespace WeddingWonderAPI.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly VnPayService _vnPayService;
        private readonly UserService _userService;
        public PaymentController(TransactionService transactionService, VnPayService vnPayService, UserService userService)
        {
            _transactionService = transactionService;
            _vnPayService = vnPayService;
            _userService = userService;
        }

        [HttpPost("createpaymenturl")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentinformationDTO model)
        {
            if (model == null)
            {
                return BadRequest("Invalid payment information.");
            }

            int userId = UserHelper.GetUserId(User);
            Console.WriteLine($"User ID: {userId}");
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext, userId);
            return Ok(new { PaymentUrl = url });
        }


        [HttpPost("add-balance")]
        public IActionResult AddBalance([FromBody] PaymentinformationDTO model)
        {
            if (model == null)
            {
                return BadRequest("Invalid payment information.");
            }
            int userId = UserHelper.GetUserId(User);
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext, userId);

            return Ok(new { PaymentUrl = paymentUrl, Message = "Balance updated successfully." });
        }

        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var query = Request.Query;
            var vnPayResponse = _vnPayService.PaymentExecute(query);

            bool success = vnPayResponse.Success == true && vnPayResponse.VnPayResponseCode == "00";
            if (!query.ContainsKey("userId") || string.IsNullOrEmpty(query["userId"]))
            {
                return BadRequest("User ID is missing from the callback URL.");
            }
            int userId = int.Parse(query["userId"]);
            double amount = double.TryParse(query["vnp_Amount"], out var parsedAmount) ? parsedAmount / 100 : 0;
            string orderId = query.ContainsKey("vnp_OrderInfo") ? query["vnp_OrderInfo"].ToString() : string.Empty;
            string paymentMethod = query.ContainsKey("vnp_CardType") ? query["vnp_CardType"].ToString() : string.Empty;
            string bankName = query.ContainsKey("vnp_BankCode") ? query["vnp_BankCode"].ToString() : string.Empty; 
            if (success)
            {
                try
                {
                    var transactionCreated = await _transactionService.CreateTransactionRequestAsync(
                        userId: userId,
                        amount: amount,
                        transactionType: "Deposit",
                        status: "2",
                        reason: "Payment successful",
                        requestDate: DateTime.Now,
                        processedDate: DateTime.Now,
                        cardHolderName: "NGUYEN VAN A",
                        cardNumber: "9704198526191432198",
                        bankName: bankName 
                    );

                    if (!transactionCreated)
                    {
                        return StatusCode(500, "Failed to create transaction request");
                    }

                    await _vnPayService.UpdateUserBalance(userId, amount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating balance: {ex.Message}");
                    return StatusCode(500, "Failed to update balance");
                }
            }

            var responseMessage = new
            {
                success = success,
                amount = amount
            };

            var responseScript = $@"
                            <script>
                                window.opener.postMessage({Newtonsoft.Json.JsonConvert.SerializeObject(responseMessage)}, '*');
                                window.close();
                            </script>";

            return Content(responseScript, "text/html");
        }
    }
}