namespace WeddingWonderAPI.Models.DTOs
{
    public class AddServiceTopicsRequest
    {
        public List<int> TopicIds { get; set; } = new List<int>();
    }
}