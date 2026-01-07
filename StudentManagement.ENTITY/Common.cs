namespace StudentManagement.ENTITY
{
    public class ApiResponse<T>
    {

        public string? Message { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public object ExtraData { get; set; } = null!;
        public int StatusCode { get; set; }

    }

    public class BllResponse<T>

    {
        public bool IsCompleted { get; set; }
        public T BllData { get; set; } 
        public string Message { get; set; }
        public object ExtraData { get; set; } = null!;
    }

    public class StudentListResponse
    {
        public int StudentId { get; set; }
        public string? Code { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? StateName { get; set; }
        public int? StateId { get; set; }
        public string? CityName { get; set; }
        public int? CityId { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateOnly? Dob { get; set; }
        public int Age { get; set; }
        public int IsActive { get; set; }
        public SubjectWiseMarksResponse[] SubjectWiseMarksList { get; set; } = Array.Empty<SubjectWiseMarksResponse>();
        public StudentDocumentResponse[] StudentWiseDocuments { get; set; } = Array.Empty<StudentDocumentResponse>();

    }

    public class SubjectWiseMarksResponse
    {

        public int MarksId { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public decimal Marks { get; set; }
        public string? Indicator { get; set; }
    }

    public class StudentDocumentResponse
    {
        public int DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public int DocumentSize { get; set; }
        public string? FileName { get; set; }
        public string? OriginalFileName { get; set; }
    }

    public class StudentUpdateRequest
    {
        public string? Indicator { get; set; }
        public int StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public DateOnly Dob { get; set; }
        public int IsActive { get; set; }
        public SubjectWiseMarksRequest[]? SubjectWiseMarks { get; set; }
        public StudentDocumentRequest[]? StudentDocuments { get; set; }
    }

    public class SubjectWiseMarksRequest
    {
        public int MarksId { get; set; }
        public int SubjectId { get; set; }
        public decimal Marks { get; set; }
        public string? Indicator { get; set; }
    }

    public class StudentDocumentRequest
    {
        public int DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public string? fileName { get; set; }
        public string? Indicator { get; set; }
        public string? OriginalFileName { get; set; }
        public string? PreviousFileName { get; set; }
    }
}
