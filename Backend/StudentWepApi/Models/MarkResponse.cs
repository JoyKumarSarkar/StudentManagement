namespace StudentWepApi.Models
{
    public class MarkResponse
    {
        public long SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public decimal Marks { get; set; }
    }
}