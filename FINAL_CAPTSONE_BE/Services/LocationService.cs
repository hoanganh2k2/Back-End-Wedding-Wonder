using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class LocationService
    {
        private readonly string _jsonFilePath;

        public LocationService(IWebHostEnvironment webHostEnvironment)
        {
            _jsonFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "data.json");
        }

        public async Task<List<ProvinceDTO>> GetProvincesAsync()
        {
            var json = await File.ReadAllTextAsync(_jsonFilePath);
            var provinces = JsonSerializer.Deserialize<List<ProvinceDTO>>(json);
            return provinces ?? new List<ProvinceDTO>();
        }

        public async Task<ProvinceDTO> GetProvinceByIdAsync(string provinceId)
        {
            var provinces = await GetProvincesAsync();
            return provinces.Find(p => p.Id == provinceId);
        }

        public async Task<DistrictDTO> GetDistrictByIdAsync(string provinceId, string districtId)
        {
            var province = await GetProvinceByIdAsync(provinceId);
            return province?.Districts.Find(d => d.Id == districtId);
        }
    }
}
