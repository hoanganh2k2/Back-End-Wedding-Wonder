
namespace BusinessObject.DTOs
{
    public class ContractDTO
    {
        public int ContractId { get; set; }
        public string Title { get; set; } = null!;
        public string PdfFilePath { get; set; } 
        public int UserId {get;set;}
        public string FrontIdCardPath { get; set; } 
        public string BackIdCardPath { get; set; } 
        public bool IsConfirmed { get; set; } 
        public DateTime SignedDate {get;set;}
        public string Otp { get; set; } = null!;

    }
}