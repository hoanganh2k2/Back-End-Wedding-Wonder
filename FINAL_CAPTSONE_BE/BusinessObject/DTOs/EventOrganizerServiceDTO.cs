namespace WeddingWonderAPI.Models.DTOs
{
    using Newtonsoft.Json;

    public class EventOrganizerServiceDTO
    {
        [JsonProperty("serviceId")]
        public int ServiceId { get; set; }

        public List<EventPackageDTO> EventPackages { get; set; } = new List<EventPackageDTO>();
    }
}