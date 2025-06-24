using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repository.IRepositories;
using Repository.Repositories;
using Services.Libraries;

namespace Services
{
    public class VnPayService
    {
        private readonly ITransactionRepository _withdrawalRequestRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public VnPayService(ITransactionRepository withdrawalRequestRepository, IConfiguration configuration, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _withdrawalRequestRepository = withdrawalRequestRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public string CreatePaymentUrl(PaymentinformationDTO model, HttpContext context, int? userId = null)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();

            var urlCallback = userId.HasValue
                ? $"{_configuration["PaymentCallBack:ReturnUrl"]}?userId={userId.Value}"
                : _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallback);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            return paymentUrl;
        }

        public PaymentresponseDTO PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }

        public async Task UpdateUserBalance(int userId, double amount)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                User user = await _userRepository.GetAsyncById(userId);
                if (user != null)
                {

                    user.Balance = (user.Balance ?? 0) + amount;

                    await _userRepository.UpdateAsync(userId, user);

                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        } 
    }
}
