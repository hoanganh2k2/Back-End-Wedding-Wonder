namespace BusinessObject.DTOs
{


    public class ContractDTOCreate
    {
        public string Title { get; set; } = null!;
        public string PdfFilePath { get; set; } // content of contract 
        public int UserId { get; set; }

    }
}