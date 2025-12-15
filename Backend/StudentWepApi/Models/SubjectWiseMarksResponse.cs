namespace StudentWepApi.Models
{
    public class SubjectWiseMarksResponse
    {

        public long MarksId { get; set; }
        public long SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public decimal Marks { get; set; }
        public string? Indicator { get; set; }
    }
}