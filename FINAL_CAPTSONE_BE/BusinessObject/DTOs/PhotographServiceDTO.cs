using BusinessObject.Models;

namespace WeddingWonderAPI.Models.DTOs
{
    public class PhotographServiceDTO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public List<PhotographPackageDTO> Packages { get; set; } = new List<PhotographPackageDTO>();
        public virtual List<Photographer> Photographers { get; } = new List<Photographer>();
    }
}