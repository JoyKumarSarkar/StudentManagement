using StudentWepApi.Data.StudentContext;
using StudentWepApi.Models;
namespace StudentWepApi.Utility
{
    public class ModelConversion

    {
        /// <summary>
        /// Convert Student DB Model To Student Get Response
        /// </summary>
        /// <param name="sTUDENT">Student DB Model</param>
        /// <returns>StudentGetResponse.</returns>
        public static StudentListResponse ConvertStudentDBModelToStudentGetResponse(student_joy student)
        {
            var studentGetResponse = new StudentListResponse()
            {
                StudentId = student.id,
                Code = student.code,
                Name = $"{student.first_name} {student.last_name}",
                firstName = student.first_name,
                lastName = student.last_name,
                StateId = student.state?.id,
                StateName = student.state?.name ?? string.Empty,
                CityId = student.city?.id,
                CityName = student.city?.name ?? string.Empty,
                Mobile = student.mobile,
                Email = student.email_id,
                Dob = student.dob,
                Age = student.age,
                IsActive = student.is_active.HasValue ? student.is_active.Value : false,
                SubjectWiseMarksList = student.student_marksList_joy
                    .Where(m => m.subject != null)
                    .Select(m => new SubjectWiseMarksResponse
                    {
                        MarksId = m.id,
                        SubjectId = m.subject_id,
                        SubjectName = m.subject?.name ?? string.Empty,
                        Marks = m.marks
                    })
                    .ToArray(),
                StudentWiseDocuments = student.document_joy
                              .Select(d => new StudentDocumentResponse
                              {
                                  DocumentId = d.id,
                                  DocumentName = d.name,
                                  DocumentType = d.type,
                                  DocumentSize = d.size,
                                  FileName = d.file_name,
                                  OriginalFileName = d.original_file_name,
                              })
                            .ToArray()

            };
            return studentGetResponse;
        }

        

        /// <summary>
        /// Convert State DB Model To State Details Response
        /// </summary>
        /// <param name="sTATE">State DB Model</param>
        /// <returns>StateResponse.</returns>
        public static StateResponse ConvertStateDBModelToStateDetailsResponse(state_joy sTATE)
        {
            var stateResponse = new StateResponse()
            {
                StateId = sTATE.id,
                StateName = sTATE.name,
                IsActive = sTATE.is_active.HasValue ? sTATE.is_active.Value : false,
            };
            return stateResponse;
        }

        /// <summary>
        /// Convert City DB Model To City Details Response
        /// </summary>
        /// <param name="cITY">City DB Model</param>
        /// <returns>CityResponse.</returns>
        public static CityResponse ConvertCityDBModelToCityDetailsResponse(city_joy cITY)
        {
            var cityResponse = new CityResponse()
            {
                CityId = cITY.id,
                CityName = cITY.name,
                StateId = cITY.state_id,
                IsActive = cITY.is_active.HasValue ? cITY.is_active.Value : false,
            };
            return cityResponse;
        }

        /// <summary>
        /// Convert Subject DB Model To Subject Details Response
        /// </summary>
        /// <param name="sUBJECT">Subject DB Model</param>
        /// <returns>SubjectResponse.</returns>
        public static SubjectResponse ConvertSubjectDBModelToSubjectDetailsResponse(subject_joy sUBJECT)
        {
            var subjectResponse = new SubjectResponse()
            {
                SubjectId = sUBJECT.id,
                SubjectName = sUBJECT.name,
                IsActive = sUBJECT.is_active.HasValue ? sUBJECT.is_active.Value : false,
            };

            return subjectResponse;
        }

        /// <summary>
        /// Convert Marks DB Model To Marks Details Response
        /// </summary>
        /// <param name="mARKS">Marks DB Model</param>
        /// <returns>MarkResponse.</returns>
        public static SubjectWiseMarksResponse ConvertMarksDBModelToSubjectWiseMarksDetailsResponse(student_marksList_joy mARKS)
        {
            var SubjectWiseMarksResponse = new SubjectWiseMarksResponse()
            {
                MarksId = mARKS.id,
                SubjectId = mARKS.subject_id,
                SubjectName = mARKS.subject?.name,
                Marks = mARKS.marks
            };
            return SubjectWiseMarksResponse;
        }

        /// <summary>
        /// Convert Document DB Model To Document Details Response
        /// </summary>
        /// <param name="dOCUMENT">Document DB Model</param>
        /// <returns>DocumentResponse.</returns>
        public static StudentDocumentResponse ConvertDocumentDBModelToStudentDocumentResponse(document_joy dOCUMENT)
        {
            var StudentDocumentResponse = new StudentDocumentResponse()
            {
                DocumentId = dOCUMENT.id,
                DocumentName = dOCUMENT.name,
                DocumentSize = dOCUMENT.size,
                DocumentType = dOCUMENT.type,
                FileName = dOCUMENT.file_name,
                OriginalFileName = dOCUMENT.original_file_name,
            };
            return StudentDocumentResponse;
        }
    }
}

