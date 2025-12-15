using StudentWepApi.Data.StudentContext;
using StudentWepApi.Models;

namespace StudentWepApi.Data
{
    public interface IStudentDataAccess
    {
        StudentResponse GetStudentList(string searchText, bool showInActive);
        List <SubjectCountResponse> GetSubjectCounttList();
        CommonResponse UpdateStudent(StudentUpdateRequest studentRequest);
        StudentDetailsResponse GetStudentDetails(long id);
        public DocumentUploadResponse UploadDocument(IFormFile file);
        public DocumentDownloadResponse DownloadDocument(string fileName);
        bool IsCityBelongsToTheState(StudentUpdateRequest studentRequest);
        public bool DoesStudentExist(long studentId);
    }
}