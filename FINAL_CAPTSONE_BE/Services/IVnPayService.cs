using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentinformationDTO model, HttpContext context);
        PaymentresponseDTO PaymentExecute(IQueryCollection collections);
        void UpdateUserBalance(int userId, double amount);
    }
}
