namespace WeddingWonderAPI.Models.DTOs
{

public class ServiceCreateUpdateRequestDTO
{
    public ServiceDTO BaseService { get; set; }
    public object SpecificServiceData { get; set; }
}
}