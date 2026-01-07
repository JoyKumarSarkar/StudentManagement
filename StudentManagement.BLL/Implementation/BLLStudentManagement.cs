using Azure;
using StudentManagement.BLL.Signature;
using StudentManagement.ENTITY;
using StudentManagement.LOGGER;
using STUDENTMANAGEMENT.DAL.DbContexts;
using STUDENTMANAGEMENT.DAL.SQLServerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Implementation
{
    public partial class BLLCommon : IBLLCommon
    {
        public readonly IApplicationLogger _logger;
        public readonly StudentManagementDbContext _dBContext;
        public BLLCommon(IApplicationLogger logger, StudentManagementDbContext dBContext)
        {
            _logger = logger;
            _dBContext = dBContext;
        }

        #region GetStudent
        public BllResponse<StudentListResponse[]> GetStudentList(string searchText = "", bool showInactive = false)
        {
            var response = new BllResponse<StudentListResponse[]>();

            try
            {
                _logger.LogBLLStart(MethodBase.GetCurrentMethod()!);
                var studentList = _dBContext.TStudent
                    .Where(student => (showInactive || student.IsActive == 1) &&
                                      (string.IsNullOrEmpty(searchText) ||
                                       student.FirstName.Contains(searchText) ||
                                       student.LastName.Contains(searchText) ||
                                       student.Code.Contains(searchText) ||
                                       student.EmailId.Contains(searchText) ||
                                       student.Mobile.Contains(searchText)))
                    .Select(student => new StudentListResponse
                    {
                        StudentId = student.Id,
                        Code = student.Code,
                        firstName = student.FirstName,
                        lastName = student.LastName,
                        StateId = student.StateId,
                        CityId = student.CityId,
                        Mobile = student.Mobile,
                        Email = student.EmailId,
                        Dob = student.Dob,
                        Age = student.Age,
                        IsActive = student.IsActive
                    })
                    .ToArray();

                response.IsCompleted = true;
                response.Message = studentList.Any() ? "Student List Found" : "Student not found";
                response.BllData = studentList;
            }
            catch (Exception ex)
            {
                _logger.ManageBllException(ex, MethodBase.GetCurrentMethod()!);
                response.IsCompleted = false;
                response.BllData = null;
                response.Message = "Failed to retrieve data files.";
            }
            _logger.LogBLLEnd(MethodBase.GetCurrentMethod()!);
            return response;
        }

        #endregion GetStudent

        #region UpdateStudent
        public BllResponse<StudentListResponse[]> UpdateStudent(StudentUpdateRequest studentUpdateRequest)
        {
            var response = new BllResponse<StudentListResponse[]>();

            try
            {
                _logger.LogBLLStart(MethodBase.GetCurrentMethod()!);
                if (studentUpdateRequest.Indicator == IndicatorEnum.Delete.ToString())
                {
                    var studentToDelete = _dBContext.TStudent.FirstOrDefault(s => s.Id == studentUpdateRequest.StudentId);
                    if (studentToDelete == null)
                    {
                        response.IsCompleted = false;
                        response.BllData = null;
                        response.Message = "Student is not present";
                    }

                    _dBContext.TStudent.Remove(studentToDelete!);
                    _dBContext.SaveChanges();

                    var studentData = _dBContext.TStudent
                        .Where(x => x.IsActive == 1)
                        .Select(x => new StudentListResponse
                        {
                            StudentId = x.Id,
                            Code = x.Code,
                            firstName = x.FirstName,
                            lastName = x.LastName,
                            StateId = x.StateId,
                            CityId = x.CityId,
                            Mobile = x.Mobile,
                            Email = x.EmailId,
                            Dob = x.Dob,
                            Age = x.Age,
                            IsActive = x.IsActive
                        })
                        .ToArray();

                    response.IsCompleted = true;
                    response.BllData = studentData;
                    response.Message = $"{studentToDelete.FirstName} {studentToDelete.LastName} deleted successfully";

                }

                if (studentUpdateRequest.Indicator == IndicatorEnum.Insert.ToString())
                {
                    var newStudent = new TStudentStage
                    {
                        FirstName = studentUpdateRequest.FirstName!,
                        LastName = studentUpdateRequest.LastName!,
                        Code = GenerateStudentCode(),
                        StateId = studentUpdateRequest.StateId,
                        CityId = studentUpdateRequest.CityId,
                        Mobile = studentUpdateRequest.Mobile!,
                        EmailId = studentUpdateRequest.Email!,
                        Dob = studentUpdateRequest.Dob,
                        Age = CalculateAge(studentUpdateRequest.Dob),
                        IsActive = studentUpdateRequest.IsActive,
                    };

                    _dBContext.TStudentStage.Add(newStudent);
                    _dBContext.SaveChanges();

                    var studentData = _dBContext.TStudent
                        .Where(x => x.IsActive == 1)
                        .Select(x => new StudentListResponse
                        {
                            StudentId = x.Id,
                            Code = x.Code,
                            firstName = x.FirstName,
                            lastName = x.LastName,
                            StateId = x.StateId,
                            CityId = x.CityId,
                            Mobile = x.Mobile,
                            Email = x.EmailId,
                            Dob = x.Dob,
                            Age = x.Age,
                            IsActive = x.IsActive
                        })
                        .ToArray();

                    response.IsCompleted = true;
                    response.Message = $"{newStudent.FirstName} {newStudent.LastName} added successfully";
                    response.BllData = studentData;
                }
                else
                {
                    var student = _dBContext.TStudent.FirstOrDefault(s => s.Id == studentUpdateRequest.StudentId);
                    if (student == null)
                    {
                        response.IsCompleted = false;
                        response.BllData = null;
                        response.Message = "Student is not present";
                    }

                    student.FirstName = studentUpdateRequest.FirstName;
                    student.LastName = studentUpdateRequest.LastName;
                    student.Mobile = studentUpdateRequest.Mobile;
                    student.EmailId = studentUpdateRequest.Email;
                    student.Dob = studentUpdateRequest.Dob;
                    student.StateId = studentUpdateRequest.StateId;
                    student.CityId = studentUpdateRequest.CityId;
                    student.Age = CalculateAge(studentUpdateRequest.Dob);
                    student.IsActive = studentUpdateRequest.IsActive;

                    _dBContext.TStudent.Update(student);
                    _dBContext.SaveChanges();

                    var studentData = _dBContext.TStudent
                        .Where(x => x.IsActive == 1)
                        .Select(x => new StudentListResponse
                        {
                            StudentId = x.Id,
                            Code = x.Code,
                            firstName = x.FirstName,
                            lastName = x.LastName,
                            StateId = x.StateId,
                            CityId = x.CityId,
                            Mobile = x.Mobile,
                            Email = x.EmailId,
                            Dob = x.Dob,
                            Age = x.Age,
                            IsActive = x.IsActive
                        })
                        .ToArray();

                    response.IsCompleted = true;
                    response.Message = $"{student.FirstName} {student.LastName} edited successfully";
                    response.BllData = studentData;
                }
            }
            catch (Exception ex)
            {
                _logger.ManageBllException(ex, MethodBase.GetCurrentMethod()!);
                response.IsCompleted = false;
                response.BllData = null;
                response.Message = "Failed to retrieve data files.";
            }
            _logger.LogBLLEnd(MethodBase.GetCurrentMethod()!);
            return response;
        }

    }

    #endregion GetStudent
}


