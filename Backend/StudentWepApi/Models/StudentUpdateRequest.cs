using System.ComponentModel.DataAnnotations;

namespace StudentWepApi.Models
{
    public class StudentUpdateRequest
    {
        public string? Indicator { get; set; }
        public long StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public long StateId { get; set; }
        public long CityId { get; set; }
        public DateOnly Dob { get; set; }
        public bool? IsActive { get; set; }
        public SubjectWiseMarksRequest[]? SubjectWiseMarks { get; set; }
        public StudentDocumentRequest[]? StudentDocuments { get; set; }
    }
}