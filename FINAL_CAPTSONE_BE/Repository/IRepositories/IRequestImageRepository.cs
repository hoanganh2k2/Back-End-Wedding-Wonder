using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IRequestImageRepository
    {
        Task<List<RequestImageDTO>> GetImagesByRequestIdAsync(int requestId);
        Task<bool> AddImagesAsync(List<RequestImage> images);
    }
}
