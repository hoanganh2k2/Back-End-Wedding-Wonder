namespace WeddingWonderAPI.Models.DTOs
{
    public class AddUserTopicsRequest
    {
        public List<int> TopicIds { get; set; } = new List<int>();
    }
}