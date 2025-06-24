using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class RequestUpgradeSupplierRepository : IRequestUpgradeSupplierRepository
    {
        private readonly RequestUpgradeSupplierDAO _dao;

        public RequestUpgradeSupplierRepository(RequestUpgradeSupplierDAO dao)
        {
            _dao = dao;
        }

        public Task<List<RequestUpgradeSupplierDTO>> GetRequestsByStatusAsync(string status) => _dao.GetRequestsByStatusAsync(status);

        public Task<RequestUpgradeSupplierDTO?> GetRequestByIdAsync(int requestId) => _dao.GetRequestByIdAsync(requestId);

        public Task<bool> AddRequestAsync(RequestUpgradeSupplier request) => _dao.AddRequestAsync(request);

        public Task<bool> UpdateRequestAsync(RequestUpgradeSupplier request) => _dao.UpdateRequestAsync(request);
    }
}
