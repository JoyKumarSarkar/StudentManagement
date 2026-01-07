using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Signature;
using StudentManagement.ENTITY;
using StudentManagement.LOGGER;
using System.Reflection;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentManagementController : ControllerBase
    {
        private readonly IApplicationLogger _Logger;
        private readonly IBLLCommon _BLL;
        private readonly IConfiguration _Config;

        public StudentManagementController(IApplicationLogger Logger, IBLLCommon BLLStudentManagement, IConfiguration Config)
        {
            _Logger = Logger;
            _BLL = BLLStudentManagement;
            _Config = Config;
        }


        #region GetStudentList
        [HttpGet]
        [Route("GetStudentList")]
        public ApiResponse<StudentListResponse[]> GetStudentList(string searchText = "", bool showInactive = false)
        {
            try
            {
                _Logger.LogApiStart(MethodBase.GetCurrentMethod()!);
                var result = _BLL.GetStudentList(searchText, showInactive);
                _Logger.LogApiEnd(MethodBase.GetCurrentMethod()!);
                if (result.IsCompleted)
                {
                    return new ApiResponse<StudentListResponse[]> { Data = result.BllData, Success = true, Message = result.Message };
                }
                else
                {
                    return new ApiResponse<StudentListResponse[]> { Data = null!, Success = false, Message = result.Message };
                }
            }

            catch (Exception ex)
            {
                _Logger.ManageApiException(ex, MethodBase.GetCurrentMethod()!);
                return new ApiResponse<StudentListResponse[]> { Data = null!, Success = false, Message = "Internal server error", StatusCode = 500 };
            }
        }

        #endregion GetStudentList

        #region UpdateStydent
        [HttpPost]
        [Route("UpdateStudent")]
        public ApiResponse<StudentListResponse[]> UpdateStudent(StudentUpdateRequest studentUpdateRequest)
        {
            try
            {
                _Logger.LogApiStart(MethodBase.GetCurrentMethod()!);
                var result = _BLL.UpdateStudent(studentUpdateRequest);
                _Logger.LogApiEnd(MethodBase.GetCurrentMethod()!);
                if (result.IsCompleted)
                {
                    return new ApiResponse<StudentListResponse[]> { Data = result.BllData, Success = true, Message = result.Message };
                }
                else
                {
                    return new ApiResponse<StudentListResponse[]> { Data = null!, Success = false, Message = result.Message };
                }
            }

            catch (Exception ex)
            {
                _Logger.ManageApiException(ex, MethodBase.GetCurrentMethod()!);
                return new ApiResponse<StudentListResponse[]> { Data = null!, Success = false, Message = "Internal server error", StatusCode = 500 };
            }
        }

        #endregion UpdateStydent
    }
}