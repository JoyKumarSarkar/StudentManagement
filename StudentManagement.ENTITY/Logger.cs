namespace StudentManagement.ENTITY
{
    public class Logger
    {
        public enum LogTypeEnum
        {
            Info = 1,
            Error = 2,
            Warning = 3,
            NotFoundInDatabase = 4
        }
        public enum LogAssemblyEnum
        {
            API = 1,
            BLL = 2,
            JOBS = 3,
        }
        public enum StartEnd
        {
            START = 1,
            END = 2,
        }

    }
}
