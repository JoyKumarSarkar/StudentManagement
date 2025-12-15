namespace StudentWepApi.Models
{
    public class StudentResponse : CommonResponse
    {
        public StudentListResponse[] StudentList { get; set; } = Array.Empty<StudentListResponse>();
    }
}