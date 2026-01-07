using System.Reflection;
using static StudentManagement.ENTITY.Logger;

namespace StudentManagement.LOGGER
{
    public interface IApplicationLogger
    {
        void InfoLog(string message, MethodBase method, LogAssemblyEnum logAssembly);
        void ManageException(Exception ex, MethodBase method, LogAssemblyEnum logAssembly);
        void LogApiStart(MethodBase method);
        void LogApiEnd(MethodBase method);
        void LogBLLStart(MethodBase method);
        void LogBLLEnd(MethodBase method);
        void ManageApiException(Exception ex, MethodBase method);
        void ManageBllException(Exception ex, MethodBase method);
        void ManageApiLog(string message, MethodBase method, LogTypeEnum logType);
        void ManageBllLog(string message, MethodBase method, LogTypeEnum logType);
    }

}
