namespace StudentWepApi.Models
{
    public class DocumentUploadRequest
    {
        public string? Indicator { get; set; }
        public long DocumentId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Size { get; set; }
        public long StudentId { get; set; }
    }
}