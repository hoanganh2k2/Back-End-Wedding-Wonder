namespace WeddingWonderAPI.Models.Response
{
    public class ApiResponse
    {
        public bool? Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
