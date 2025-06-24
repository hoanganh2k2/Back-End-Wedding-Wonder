using BusinessObject.Models;

namespace WeddingWonderAPI.Models.DTOs
{
    public class MakeUpServiceDTO
    {
        public int ServiceId { get; set; }
        public List<MakeUpPackageDTO> Packages { get; set; } = new List<MakeUpPackageDTO>();
        public virtual List<MakeUpArtist> MakeUpArtists { get; } = new List<MakeUpArtist>();
    }
}