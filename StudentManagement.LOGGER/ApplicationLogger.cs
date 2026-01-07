
using Microsoft.Extensions.Logging;
using StudentManagement.LOGGER;
using STUDENTMANAGEMENT.DAL.DbContexts;
using STUDENTMANAGEMENT.DAL.SQLServerModels;
using System.Reflection;
using static StudentManagement.ENTITY.Logger;

namespace StudentManagement.LOGGER
{

    public class ApplicationLogger : IApplicationLogger
    {
        private readonly StudentManagementDbContext _dbContext;
        private readonly ILogger<ApplicationLogger> _iLogger;


        public ApplicationLogger(StudentManagementDbContext _DbContext, ILogger<ApplicationLogger> _ILogger)
        {
            _dbContext = _DbContext;
            _iLogger = _ILogger;
        }

        public void InfoLog(string message, MethodBase method, LogAssemblyEnum logAssembly)
        {
            WriteLog(
                message,
                LogTypeEnum.Info,
                method,
                logAssembly
            );
        }

        public void ManageException(Exception ex, MethodBase method, LogAssemblyEnum logAssembly)
        {
            var logMessage = $@"
                            Exception Message: {ex.Message}
                            StackTrace: {ex.StackTrace}
                            InnerException: {ex.InnerException?.Message}";

            WriteLog(
                logMessage,
                LogTypeEnum.Error,
                method,
                logAssembly
            );
        }

        private void WriteLog(string message, LogTypeEnum logType, MethodBase method, LogAssemblyEnum logAssembly)
        {
            try
            {
                var log = new TApplicationLog
                {
                    LogDetails = message,
                    LogType = (int)logType,
                    LogDate = DateTime.Now,
                    Root = method.Name,
                    LogAssembly = (int)logAssembly
                };

                _dbContext.Add(log);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // In a real-world scenario, consider handling logging failures appropriately.
                _iLogger.LogError(ex.Message);
            }
        }

        public void LogApiStart (MethodBase method)
        {
            InfoLog(StartEnd.START.ToString(), method, LogAssemblyEnum.API);
        }
        public void LogApiEnd (MethodBase method)
        {
            InfoLog(StartEnd.END.ToString(), method, LogAssemblyEnum.API);
        }
        public void LogBLLStart (MethodBase method)
        {
            InfoLog(StartEnd.START.ToString(), method, LogAssemblyEnum.BLL);
        }
        public void LogBLLEnd (MethodBase method)
        {
            InfoLog(StartEnd.END.ToString(), method, LogAssemblyEnum.BLL);
        }
        public void ManageApiException(Exception ex, MethodBase method)
        {
            ManageException(ex, method, LogAssemblyEnum.API);
        }
        public void ManageBllException(Exception ex, MethodBase method)
        {
            ManageException(ex, method, LogAssemblyEnum.BLL);
        }
        public void ManageApiLog(string message, MethodBase method, LogTypeEnum logType)
        {
            WriteLog(message,  logType, method, LogAssemblyEnum.API);
        }
        public void ManageBllLog(string message, MethodBase method, LogTypeEnum logType)
        {
            WriteLog(message,  logType, method, LogAssemblyEnum.BLL);
        }
    }
}
