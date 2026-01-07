using StudentManagement.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Signature
{
    public interface IBLLCommon
    {
        public BllResponse<StudentListResponse[]> GetStudentList(string searchText = "", bool showInactive = false);
        public BllResponse<StudentListResponse[]> UpdateStudent(StudentUpdateRequest studentUpdateRequest);

        string GenerateStudentCode();
        int CalculateAge(DateOnly Dob);

        Task RunAsync(CancellationToken cancellationToken);

    }
}
