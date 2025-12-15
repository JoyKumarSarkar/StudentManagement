namespace StudentWepApi.Models
{
    public class SubjectWiseMarksRequest
    {
        public long MarksId { get; set; }
        public long SubjectId { get; set; }
        public decimal Marks { get; set; }
        public string? Indicator { get; set; }
    }
}