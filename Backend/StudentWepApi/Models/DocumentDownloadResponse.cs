namespace StudentWepApi.Models
{
    public class DocumentDownloadResponse : CommonResponse
    {
        public byte[] FileBytes { get; set; }
        public string ContentType { get; set; }
        public string OriginalFileName { get; set; }
    }
}
