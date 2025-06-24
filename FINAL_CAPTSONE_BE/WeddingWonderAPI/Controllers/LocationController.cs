using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("provinces")]
        public async Task<ActionResult> GetProvinces()
        {
            var provinces = await _locationService.GetProvincesAsync();
            return Ok(provinces);
        }

        [HttpGet("provinces/{provinceId}/districts")]
        public async Task<ActionResult> GetDistricts(string provinceId)
        {
            var province = await _locationService.GetProvinceByIdAsync(provinceId);
            if (province == null)
            {
                return NotFound(new { message = "Province not found" });
            }

            return Ok(province.Districts);
        }

        [HttpGet("provinces/{provinceId}/districts/{districtId}/wards")]
        public async Task<ActionResult> GetWards(string provinceId, string districtId)
        {
            var district = await _locationService.GetDistrictByIdAsync(provinceId, districtId);
            if (district == null)
            {
                return NotFound(new { message = "District not found" });
            }

            return Ok(district.Wards);
        }

    }
}
