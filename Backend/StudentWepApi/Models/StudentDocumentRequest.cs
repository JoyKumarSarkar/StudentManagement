
namespace StudentWepApi.Models
{
    public class StudentDocumentRequest
    {
        public long DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public string? fileName { get; set; }
        public string? Indicator { get; set; }
        public string? OriginalFileName { get; set; }
        public string? PreviousFileName { get; set; } 
    }
}