namespace StudentWepApi.Models
{
    public class StudentListResponse
    {
        public long StudentId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? StateName { get; set; }
        public long? StateId { get; set; }
        public string? CityName { get; set; }
        public long? CityId { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateOnly? Dob { get; set; }
        public long? Age { get; set; }
        public bool? IsActive { get; set; }
        public SubjectWiseMarksResponse[] SubjectWiseMarksList { get; set; } = Array.Empty<SubjectWiseMarksResponse>();
        public StudentDocumentResponse[] StudentWiseDocuments { get; set; } = Array.Empty<StudentDocumentResponse>();

    }
}