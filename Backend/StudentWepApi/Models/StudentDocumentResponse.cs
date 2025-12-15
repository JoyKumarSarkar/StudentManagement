

namespace StudentWepApi.Models
{
    public class StudentDocumentResponse
    {
        public long DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public int DocumentSize { get; set; }
        public string? FileName { get; set; }
        public string? OriginalFileName { get; set; }
    }
}