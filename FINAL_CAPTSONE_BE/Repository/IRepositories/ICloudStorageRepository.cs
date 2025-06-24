using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface ICloudStorageRepository
    {
        Task<string> UploadFileAsync(string userId, Stream fileStream, string fileName, string contentType);
    }
}
