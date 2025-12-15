//namespace StudentWepApi.Models
//{
//    public class DocumentUploadResponse : CommonResponse
//    {
//        public string FileName { get; set; }
//        public string OriginalFileName { get; set; }
//    }
//}



namespace StudentWepApi.Models
{
    public class DocumentUploadResponse : CommonResponse
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public long DocumentSize { get; set; }
    }
}