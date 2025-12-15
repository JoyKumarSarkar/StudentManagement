
using StudentWepApi.Models;

namespace StudentWepApi.Models
{
    public class StudentDetailsResponse : CommonResponse
    {
        public StateResponse[] StateList { get; set; } = Array.Empty<StateResponse>();
        public CityResponse[] CityList { get; set; } = Array.Empty<CityResponse>();
        public SubjectResponse[] SubjectList { get; set; } = Array.Empty<SubjectResponse>();
        public SubjectWiseMarksResponse[] SubjectWiseMarks { get; set; } = Array.Empty<SubjectWiseMarksResponse>();
        public StudentDocumentResponse[] StudentWiseDocuments { get; set; } = Array.Empty<StudentDocumentResponse>();

    }
}