using StudentManagement.BLL.Signature;
using StudentManagement.LOGGER;
using STUDENTMANAGEMENT.DAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Implementation
{
    public partial class BLLCommon : IBLLCommon
    {

        //public readonly StudentManagementDbContext _dBContext;
        //public BLLCommon(StudentManagementDbContext dBContext)
        //{
        //    _dBContext = dBContext;
        //}
        public string GenerateStudentCode()
        {
            var students = _dBContext.TStudent
                .Select(s => new { s.Code })
                .ToList();

            string code;

            if (students.Any())
            {
                var maxCode = students
                    .Select(s => int.TryParse(s.Code.Substring(1), out var num) ? num : 0)
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

        public int CalculateAge(DateOnly dob)
        {
            if (dob == null)
            {
                throw new ArgumentNullException(nameof(dob), "Date of birth cannot be null.");
            }
            var today = DateOnly.FromDateTime(DateTime.Today);
            var birthDate = dob;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }
    }
}
