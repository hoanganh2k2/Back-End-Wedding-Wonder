namespace WeddingWonderAPI.Models.DTO
{
    public class WeddingCardServiceDTO
    {
        public int ServiceId { get; set; }
        public List<InvitationPackageDTO> CardPackages { get; set; } = new List<InvitationPackageDTO>();
    }
}