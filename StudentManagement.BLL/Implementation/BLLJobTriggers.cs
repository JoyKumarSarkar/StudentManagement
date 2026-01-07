using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.Signature;
using StudentManagement.ENTITY;
using StudentManagement.LOGGER;
using STUDENTMANAGEMENT.DAL.SQLServerModels;
using System.Data;
using System.Reflection;
using static Azure.Core.HttpHeader;
using static StudentManagement.ENTITY.Logger;

namespace StudentManagement.BLL.Implementation
{
    public partial class BLLCommon : IBLLCommon
    {
        public BLLCommon(IApplicationLogger Logger)
        {
            _logger = Logger;
        }
        public async Task RunAsync(CancellationToken cancellationToken)
        {


            _logger.InfoLog(
                    "Copying Student data from TStudent to TStudentStage started",
                    MethodBase.GetCurrentMethod()!,
                    LogAssemblyEnum.JOBS
                );
            try
            {
                var stagedStudents = await _dBContext.TStudentStage
                    .Where(x => x.IsStage == (int)IsStaged.False)
                    .ToListAsync(cancellationToken);

                if (!stagedStudents.Any())
                    return;

                var students = stagedStudents.Select(x => new TStudent
                {
                    Code = x.Code,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    StateId = x.StateId,
                    CityId = x.CityId,
                    Mobile = x.Mobile,
                    EmailId = x.EmailId,
                    Dob = x.Dob,
                    Age = x.Age,
                    IsActive = x.IsActive
                }).ToList();

                await _dBContext.TStudent.AddRangeAsync(students, cancellationToken);

                stagedStudents.ForEach(x => x.IsStage = (int)IsStaged.True);

                await _dBContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ManageException(
                       ex,
                       MethodBase.GetCurrentMethod()!,
                       LogAssemblyEnum.JOBS
                   );
            }
            _logger.InfoLog(
                    "Copying Student data from TStudent to TStudentStage completed",
                    MethodBase.GetCurrentMethod()!,
                    LogAssemblyEnum.JOBS
                );
        }
    }
}
