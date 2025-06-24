using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IRequestUpgradeSupplierRepository
    {
        Task<List<RequestUpgradeSupplierDTO>> GetRequestsByStatusAsync(string status);
        Task<RequestUpgradeSupplierDTO?> GetRequestByIdAsync(int requestId);
        Task<bool> AddRequestAsync(RequestUpgradeSupplier request);
        Task<bool> UpdateRequestAsync(RequestUpgradeSupplier request);
    }
}
