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
    public class RequestImageRepository : IRequestImageRepository
    {
        private readonly RequestImageDAO _dao;

        public RequestImageRepository(RequestImageDAO dao)
        {
            _dao = dao;
        }

        public Task<List<RequestImageDTO>> GetImagesByRequestIdAsync(int requestId) => _dao.GetImagesByRequestIdAsync(requestId);

        public Task<bool> AddImagesAsync(List<RequestImage> images) => _dao.AddImagesAsync(images);
    }
}
