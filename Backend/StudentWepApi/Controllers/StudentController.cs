using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentWepApi.Data;
using StudentWepApi.Data.StudentContext;
using StudentWepApi.Models;
using StudentWepApi.Utility;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StudentWepApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentDataAccess _studentDataAccess;

        public StudentController(ILogger<StudentController> logger, IStudentDataAccess studentDataAccess)
        {
            _logger = logger;
            _studentDataAccess = studentDataAccess;
        }


        ///<summary>
        /// Get filtered student list besed on search text and activity
        ///</summary>
        /// <param name="searchText">Search keyword (optional)</param>
        /// <param name="showInActive">Whether to include inactive students (default: false)</param>
        ///<returns>Filtered list of students <seealso cref="StudentResponse"/> StudentResponse.</returns>
        [HttpGet("GetStudentList")]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetStudentList(string searchText = "", bool showInactive = false)
        {
            ObjectResult objectResult;

            try
            {
                var result = _studentDataAccess.GetStudentList(searchText, showInactive);
                objectResult = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return objectResult;
        }



        [HttpGet("GetSubjectCountList")]
        [ProducesResponseType(typeof(SubjectCountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetSubjectCounttList()
        {
            ObjectResult objectResult;

            try
            {
                var result = _studentDataAccess.GetSubjectCounttList();
                objectResult = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return objectResult;
        }


        ///<summary>
        /// Update a student
        ///</summary>
        ///<param name="studentRequest">Student Data</param>
        ///<returns>Updated student details <seealso cref="CommonResponse"/> CommonResponse.</returns>
        [HttpPost("UpdateStudent")]
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult UpdateStudent(StudentUpdateRequest studentRequest)
        {
            ObjectResult objectResult;
            var response = new CommonResponse();
            try
            {
                if (studentRequest == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Request body is null";
                    objectResult = new ObjectResult(response)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    return objectResult;
                }

                if (studentRequest.Indicator != "D")
                {
                    if (!Commonn.IsEmptyFirstName(studentRequest.FirstName))
                    {
                        response.IsSuccess = false;
                        response.Message = "First Name can not be empty";
                        objectResult = new ObjectResult(response)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return objectResult;
                    }

                    if (!Commonn.IsEmptyLastName(studentRequest.LastName))
                    {
                        response.IsSuccess = false;
                        response.Message = "Last Name can not be empty";
                        objectResult = new ObjectResult(response)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return objectResult;
                    }

                    if (!Commonn.IsValidMobile(studentRequest.Mobile))
                    {
                        response.IsSuccess = false;
                        response.Message = "Invalid or missing mobile number.";
                        objectResult = new ObjectResult(response)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return objectResult;
                    }

                    if (!Commonn.IsValidEmail(studentRequest.Email))
                    {
                        response.IsSuccess = false;
                        response.Message = "Invalid or missing email address.";
                        objectResult = new ObjectResult(response)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return objectResult;
                    }

                    if (!_studentDataAccess.IsCityBelongsToTheState(studentRequest))
                    {
                        response.IsSuccess = false;
                        response.Message = "Selected city Id does not belong to the selected state Id.";
                        objectResult = new ObjectResult(response)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return objectResult;
                    }
                }

                var result = _studentDataAccess.UpdateStudent(studentRequest);
                objectResult = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                return objectResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        ///<summary>
        /// Get student details including state, city, subjects, and marks if ID > 0
        ///</summary>
        ///<param name="id">Student Id</param>
        ///<returns>Student details <seealso cref="StudentDetailsResponse"/> StudentGetResponse.</returns>

        [HttpGet("GetStudentDetails/{id}")]
        [ProducesResponseType(typeof(StudentDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]

        public IActionResult GetStudentDetails(long id)
        {
            ObjectResult objectResult;

            try
            {
                var result = _studentDataAccess.GetStudentDetails(id);
                objectResult = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                return objectResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("UploadDocument")]
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult UploadDocument(IFormFile file)
        {
            ObjectResult objectResult;
            try
            {
                var result = _studentDataAccess.UploadDocument(file);
                objectResult = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                return objectResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("DownloadDocument/{fileName}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public IActionResult DownloadDocument(string fileName)
        {
            ObjectResult objectResult;
            var result = _studentDataAccess.DownloadDocument(fileName);
            try
            {
                if (result == null || result.FileBytes == null)
                {
                    var response = new CommonResponse();
                    response.IsSuccess = false;
                    response.Message = "File not found.";
                    objectResult = new ObjectResult(response)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    return objectResult;
                }

                objectResult = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                return objectResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
