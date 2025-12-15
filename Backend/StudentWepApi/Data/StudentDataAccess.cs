
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentWepApi.Controllers;
using StudentWepApi.Data.StudentContext;
using StudentWepApi.Models;
using StudentWepApi.Utility;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StudentWepApi.Data
{
    public class StudentDataAccess : IStudentDataAccess
    {
        private readonly ILogger<StudentDataAccess> _logger;
        private readonly StudentDbContext studentDBContext;
        public StudentDataAccess(ILogger<StudentDataAccess> logger, StudentDbContext _studentDBContext)
        {
            _logger = logger;
            studentDBContext = _studentDBContext;
        }

        /// <summary>   
        /// Get Student List
        /// </summary>
        /// <returns>StudentResponse.</returns>

        public StudentResponse GetStudentList(string searchText, bool showInActive)
        {
            var studentResponse = new StudentResponse();

            try
            {
                var query = studentDBContext.student_joy
                    .Include(x => x.state)
                    .Include(x => x.city)
                    .Include(x => x.student_marksList_joy)
                        .ThenInclude(m => m.subject)
                    .AsQueryable();

                if (!showInActive)
                {
                    query = query.Where(s => s.is_active == true);
                }

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string loweredSearch = searchText.ToLower();
                    query = query.Where(s =>
                        s.first_name.ToLower().Contains(loweredSearch) ||
                        s.last_name.ToLower().Contains(loweredSearch) ||
                        s.mobile.ToLower().Contains(loweredSearch) ||
                        s.email_id.ToLower().Contains(loweredSearch) ||
                        s.code.ToLower().Contains(loweredSearch) ||
                        (s.state != null && s.state.name.ToLower().Contains(loweredSearch)) ||
                        (s.city != null && s.city.name.ToLower().Contains(loweredSearch))
                    );
                }

                var studentList = query
                    .Include(x => x.document_joy)
                    .Select(x => ModelConversion.ConvertStudentDBModelToStudentGetResponse(x))
                    .ToArray();

                studentResponse.IsSuccess = true;
                studentResponse.Message = studentList.Any() ? "Student List Found" : "No students matched the criteria";
                studentResponse.StudentList = studentList;
            }
            catch (SqlException ex)
            {
                    _logger.LogError(ex, "SQL Error in GetStudentList");
                    throw;
            }

            return studentResponse;
        }

        public List<SubjectCountResponse> GetSubjectCounttList()
        {
            var subjectCountResponse = new SubjectCountResponse();

            try
            {
                var students = studentDBContext.student_joy
                    .Include(s => s.student_marksList_joy)
                        .ThenInclude(m => m.subject)
                        .ToList();

                var totalSubject = students
                    .SelectMany(s => s.student_marksList_joy)
                    .Count();

                var studentsTable = studentDBContext.student_joy.Where(s => s.is_active == true);
                var subjectGroup = studentDBContext.student_marksList_joy.GroupBy(x => x.subject_id)
                    .Select(x => new SubjectCountResponse
                    {

                        SubjectName = studentDBContext.subject_joy.Where(s => s.id == x.Key && s.is_active == true).FirstOrDefault()!.name,
                        StudentCount = x.Count(),
                        Percentage = (x.Count() * 100 / totalSubject).ToString("F2") + "%",
                        Srfsc = x.Select(s => new StudentResponseForSubjectCount
                        {
                            Name = studentsTable.Where(st => st.id == s.student_id).FirstOrDefault()!.first_name + " " + studentsTable.Where(st => st.id == s.student_id).FirstOrDefault()!.last_name,
                            Code = studentsTable.Where(st => st.id == s.student_id).FirstOrDefault()!.code,
                        }).ToArray()
                    }).ToList();

                return subjectGroup;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL Error in GetStudentList");
                throw;
            }

            //return SubjectCountResponse;
        }

        public CommonResponse UpdateStudent(StudentUpdateRequest studentRequest)
        {
            var response = new CommonResponse();

            try
            {
                if (studentRequest.Indicator == "D")
                {
                    var studentToDelete = studentDBContext.student_joy.FirstOrDefault(s => s.id == studentRequest.StudentId);
                    if (studentToDelete == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "StudentID does not exist.";
                        return response;
                    }

                    _ = studentDBContext.student_joy.Remove(studentToDelete);
                    _ = studentDBContext.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = $"{studentToDelete.first_name} {studentToDelete.last_name} deleted successfully.";
                    return response;
                }

                if (studentRequest.StudentId == 0)
                {
                    var generatedCode = GenerateStudentCode(studentDBContext);

                    var student = new student_joy
                    {
                        code = generatedCode,
                        first_name = studentRequest.FirstName,
                        last_name = studentRequest.LastName,
                        mobile = studentRequest.Mobile,
                        email_id = studentRequest.Email,
                        dob = studentRequest.Dob,
                        state_id = studentRequest.StateId,
                        city_id = studentRequest.CityId,
                        age = Commonn.CalculateAge(studentRequest.Dob),
                        is_active = studentRequest.IsActive,
                    };

                    _ = studentDBContext.student_joy.Add(student);
                    _ = studentDBContext.SaveChanges();

                    if (studentRequest.SubjectWiseMarks != null && studentRequest.SubjectWiseMarks.Any())
                    {
                        foreach (var subjectMark in studentRequest.SubjectWiseMarks)
                        {
                            var studentMarks = new student_marksList_joy
                            {
                                student_id = student.id,
                                subject_id = subjectMark.SubjectId,
                                marks = subjectMark.Marks
                            };
                            _ = studentDBContext.student_marksList_joy.Add(studentMarks);
                        }
                        _ = studentDBContext.SaveChanges();
                    }

                    if (studentRequest.StudentDocuments != null && studentRequest.StudentDocuments.Any())
                    {
                        foreach (var doc in studentRequest.StudentDocuments)
                        {
                            var uploadDirectoryPath = ConfigurationHelpers.RootFilePath;
                            var filePath = Path.Combine(uploadDirectoryPath, doc.fileName ?? "");
                            var fileSize = File.Exists(filePath) ? new FileInfo(filePath).Length : 0;

                            var newDoc = new document_joy
                            {
                                name = doc.DocumentName,
                                type = doc.DocumentType,
                                size = (int)fileSize,
                                file_name = doc.fileName,
                                student_id = student.id,
                                original_file_name = doc.OriginalFileName,
                            };
                            _ = studentDBContext.document_joy.Add(newDoc);
                        }
                        _ = studentDBContext.SaveChanges();
                    }
                    response.IsSuccess = true;
                    response.Message = $"{student.first_name} {student.last_name} added successfully.";
                }
                else
                {
                    var student = studentDBContext.student_joy.FirstOrDefault(s => s.id == studentRequest.StudentId);
                    if (student == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "StudentID does not exist.";
                        return response;
                    }

                    student.first_name = studentRequest.FirstName;
                    student.last_name = studentRequest.LastName;
                    student.mobile = studentRequest.Mobile;
                    student.email_id = studentRequest.Email;
                    student.dob = studentRequest.Dob;
                    student.state_id = studentRequest.StateId;
                    student.city_id = studentRequest.CityId;
                    student.age = Commonn.CalculateAge(studentRequest.Dob);
                    student.is_active = studentRequest.IsActive;

                    _ = studentDBContext.student_joy.Update(student);
                    _ = studentDBContext.SaveChanges();

                    if (studentRequest.SubjectWiseMarks != null && studentRequest.SubjectWiseMarks.Any())
                    {
                        foreach (var subjectMark in studentRequest.SubjectWiseMarks)
                        {
                            if (subjectMark.Indicator == "D")
                            {
                                var markToDelete = studentDBContext.student_marksList_joy
                                    .FirstOrDefault(m => m.id == subjectMark.MarksId);
                                if (markToDelete != null)
                                {
                                    _ = studentDBContext.student_marksList_joy.Remove(markToDelete);
                                }
                            }
                            else if (subjectMark.MarksId == 0)
                            {
                                var newMark = new student_marksList_joy
                                {
                                    student_id = student.id,
                                    subject_id = subjectMark.SubjectId,
                                    marks = subjectMark.Marks
                                };
                                _ = studentDBContext.student_marksList_joy.Add(newMark);
                            }
                            else
                            {
                                var existingMark = studentDBContext.student_marksList_joy
                                    .FirstOrDefault(m => m.id == subjectMark.MarksId);
                                if (existingMark != null)
                                {
                                    existingMark.subject_id = subjectMark.SubjectId;
                                    existingMark.marks = subjectMark.Marks;
                                    _ = studentDBContext.student_marksList_joy.Update(existingMark);
                                }
                            }
                        }
                        _ = studentDBContext.SaveChanges();
                    }

                    if (studentRequest.StudentDocuments != null && studentRequest.StudentDocuments.Any())
                    {
                        foreach (var doc in studentRequest.StudentDocuments)
                        {
                            if (doc.Indicator == "D")
                            {
                                string studentDirectoryPath = Path.Combine(ConfigurationHelpers.SpecificStudentFilePath, student.code);
                                string filePath = Path.Combine(studentDirectoryPath, doc.fileName ?? "");

                                try
                                {
                                    if (File.Exists(filePath))
                                    {
                                        File.Delete(filePath);

                                        var docToDelete = studentDBContext.document_joy.FirstOrDefault(d => d.file_name == doc.fileName && d.student_id == student.id);
                                        if (docToDelete != null)
                                        {
                                            studentDBContext.document_joy.Remove(docToDelete);
                                            _ = studentDBContext.SaveChanges();
                                        }

                                        response.IsSuccess = true;
                                        response.Message = "File deleted successfully.";
                                    }
                                    else
                                    {
                                        response.IsSuccess = false;
                                        response.Message = "File does not exist in.";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error deleting the file.");
                                    response.IsSuccess = false;
                                    response.Message = "Failed to delete the file: " + ex.Message;
                                }
                            }
                            else if (doc.Indicator == "I")
                            {
                                var uploadDirectoryPath = ConfigurationHelpers.TempFilePath;
                                var filePath = Path.Combine(uploadDirectoryPath, doc.fileName ?? "");

                                if (File.Exists(filePath))
                                {
                                    string studentDirectoryPath = Path.Combine(ConfigurationHelpers.SpecificStudentFilePath, student.code);
                                    if (!Directory.Exists(studentDirectoryPath))
                                    {
                                        Directory.CreateDirectory(studentDirectoryPath);
                                    }

                                    string destinationFilePath = Path.Combine(studentDirectoryPath, doc.fileName ?? "");

                                    try
                                    {
                                        File.Move(filePath, destinationFilePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Error moving file.");
                                        response.IsSuccess = false;
                                        response.Message = "Failed to move the file: " + ex.Message;
                                        return response;
                                    }

                                    var fileSize = new FileInfo(destinationFilePath).Length;

                                    var newDoc = new document_joy
                                    {
                                        name = doc.DocumentName,
                                        type = doc.DocumentType,
                                        size = (int)fileSize,
                                        file_name = doc.fileName,
                                        student_id = student.id,
                                        original_file_name = doc.OriginalFileName,
                                    };

                                    _ = studentDBContext.document_joy.Add(newDoc);
                                    _ = studentDBContext.SaveChanges();
                                }
                                else
                                {
                                    response.IsSuccess = false;
                                    response.Message = "File not found in the TempFiles directory.";
                                    return response;
                                }
                            }

                            else if (doc.Indicator == "U")
                            {
                                var uploadDirectoryPath = ConfigurationHelpers.TempFilePath;
                                var tempFilePath = Path.Combine(uploadDirectoryPath, doc.fileName ?? "");

                                if (File.Exists(tempFilePath))
                                {
                                    string studentDirectoryPath = Path.Combine(ConfigurationHelpers.SpecificStudentFilePath, student.code);
                                    if (!Directory.Exists(studentDirectoryPath))
                                    {
                                        Directory.CreateDirectory(studentDirectoryPath);
                                    }

                                    string oldFilePath = Path.Combine(studentDirectoryPath, doc.PreviousFileName ?? "");
                                    if (File.Exists(oldFilePath))
                                    {
                                        File.Delete(oldFilePath);
                                    }

                                    string newFilePath = Path.Combine(studentDirectoryPath, doc.fileName ?? "");

                                    try
                                    {
                                        File.Move(tempFilePath, newFilePath);
                                        var fileSize = new FileInfo(newFilePath).Length;

                                        var existingDoc = studentDBContext.document_joy
                                            .FirstOrDefault(d => d.file_name == doc.PreviousFileName);

                                        if (existingDoc != null)
                                        {
                                            existingDoc.name = doc.DocumentName;
                                            existingDoc.type = doc.DocumentType;
                                            existingDoc.size = (int)fileSize;
                                            existingDoc.student_id = student.id;
                                            existingDoc.file_name = doc.fileName; // update to new file name
                                            existingDoc.original_file_name = doc.OriginalFileName;

                                            _ = studentDBContext.document_joy.Update(existingDoc);
                                        }
                                        else
                                        {
                                            var newDoc = new document_joy
                                            {
                                                name = doc.DocumentName,
                                                type = doc.DocumentType,
                                                size = (int)fileSize,
                                                file_name = doc.fileName,
                                                student_id = student.id,
                                                original_file_name = doc.OriginalFileName,
                                            };

                                            _ = studentDBContext.document_joy.Add(newDoc);
                                        }

                                        _ = studentDBContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Error replacing the file.");
                                        response.IsSuccess = false;
                                        response.Message = "Failed to replace the file: " + ex.Message;
                                        return response;
                                    }
                                }
                                else
                                {
                                    response.IsSuccess = false;
                                    response.Message = "File not found in the TempFiles directory.";
                                    return response;
                                }
                            }


                        }
                        _ = studentDBContext.SaveChanges();
                    }

                    response.IsSuccess = true;
                    response.Message = $"{student.first_name} {student.last_name} updated successfully.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateStudent");
                response.IsSuccess = false;
                response.Message = "An error occurred: " + ex.Message;
            }

            return response;
        }

        public StudentDetailsResponse GetStudentDetails(long id)
        {
            var response = new StudentDetailsResponse();
            try
            {

                var studentStateId = studentDBContext.student_joy
                    .Where(s => s.id == id)
                    .Select(s => s.state_id)
                    .FirstOrDefault();

                response.StateList = studentDBContext.state_joy
                    .Where(s => s.is_active == true || s.id == studentStateId)
                    .Select(x => ModelConversion.ConvertStateDBModelToStateDetailsResponse(x))
                    .ToArray();

                var studentCityId = studentDBContext.student_joy
                    .Where(c => c.id == id)
                    .Select(c => c.city_id)
                    .FirstOrDefault();

                response.CityList = studentDBContext.city_joy
                    .Where(c => c.is_active == true || c.id == studentCityId)
                    .Select(x => ModelConversion.ConvertCityDBModelToCityDetailsResponse(x))
                    .ToArray();

                response.SubjectList = studentDBContext.subject_joy
                    .Where(s => s.is_active == true)
                    .Select(x => ModelConversion.ConvertSubjectDBModelToSubjectDetailsResponse(x))
                    .ToArray();

                if (id > 0)
                {
                    response.SubjectWiseMarks = studentDBContext.student_marksList_joy
                        .Include(m => m.subject)
                        .Where(m => m.student_id == id)
                        .Select(x => ModelConversion.ConvertMarksDBModelToSubjectWiseMarksDetailsResponse(x))
                        .ToArray();

                    response.StudentWiseDocuments = studentDBContext.document_joy
                        .Where(d => d.student_id == id)
                        .Select(x => ModelConversion.ConvertDocumentDBModelToStudentDocumentResponse(x))
                        .ToArray();
                }

                response.IsSuccess = true;
                response.Message = "Data fetched successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student details");
                response.IsSuccess = false;
                response.Message = "An error occurred while fetching data.";
            }

            return response;
        }


        public DocumentUploadResponse UploadDocument(IFormFile? file)
        {
            var response = new DocumentUploadResponse();

            try
            {
                var basePath = ConfigurationHelpers.RootFilePath;
                var uploadsFolder = ConfigurationHelpers.TempFilePath;
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileInfo = new FileInfo(file.FileName);
                string originalFileName = file.FileName;
                string fileName = $"{Guid.NewGuid()}{fileInfo.Extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var savedFileInfo = new FileInfo(filePath);
                long fileSize = savedFileInfo.Length;

                response.IsSuccess = true;
                response.Message = "File uploaded successfully.";
                response.FileName = fileName;
                response.OriginalFileName = originalFileName;
                response.DocumentSize = fileSize;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadDocument");
                response.IsSuccess = false;
                response.Message = "An error occurred while uploading file.";
            }

            return response;
        }


        public DocumentDownloadResponse DownloadDocument(string fileName)
        {
            var response = new DocumentDownloadResponse();
            try
            {
                var document = studentDBContext.document_joy
                    .FirstOrDefault(d => d.file_name == fileName);

                if (document == null)
                    return null;

                var student = studentDBContext.student_joy
                    .FirstOrDefault(s => s.id == document.student_id);

                if (student == null)
                    return null;

                string studentDirectory = Path.Combine(ConfigurationHelpers.SpecificStudentFilePath, student.code);
                string filePath = Path.Combine(studentDirectory, fileName);

                if (!File.Exists(filePath))
                    return null;

                byte[] fileBytes = File.ReadAllBytes(filePath);
                string contentType = GetContentType(filePath);

                response.IsSuccess = true;
                response.Message = "File fetched successfully.";
                response.FileBytes = fileBytes;
                response.ContentType = contentType;
                response.OriginalFileName = document.original_file_name ?? fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DownloadDocument");
                response.IsSuccess = false;
                response.Message = "An error occurred while fetching data.";
            }
            return response;
        }

        private string GetContentType(string path)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public string GenerateStudentCode(StudentDbContext context)
        {
            var students = context.student_joy
                .Select(s => new { s.code })
                .ToList();

            string code;

            if (students.Any())
            {
                var maxCode = students
                    .Select(s => int.TryParse(s.code.Substring(1), out var num) ? num : 0)
                    .DefaultIfEmpty(0)
                    .Max();

                code = $"S{(maxCode + 1).ToString("D5")}";
            }
            else
            {
                code = "S00001";
            }

            return code;
        }

        public bool IsCityBelongsToTheState(StudentUpdateRequest studentRequest)
        {
            var city = studentDBContext.city_joy.FirstOrDefault(c => c.id == studentRequest.CityId);
            return city != null || city?.state_id == studentRequest.StateId;
        }

        public bool DoesStudentExist(long studentId)
        {
            return studentDBContext.student_joy.Any(s => s.id == studentId);
        }
    }
}
