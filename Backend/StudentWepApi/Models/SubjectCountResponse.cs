namespace StudentWepApi.Models
{
    public class SubjectCountResponse
    {
        public string? SubjectName { get; set; }
        public string? Percentage { get; set; }
        public int StudentCount { get; set; }
        public StudentResponseForSubjectCount [] Srfsc { get; set; } = Array.Empty<StudentResponseForSubjectCount>();
    }
}
 