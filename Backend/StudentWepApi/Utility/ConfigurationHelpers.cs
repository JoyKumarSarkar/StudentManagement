namespace StudentWepApi.Utility
{
    public static class ConfigurationHelpers
    {
        private static IConfigurationRoot? config;
        private static string? studentDbInfoConnectionString;

        private static IConfigurationRoot Config => config ??= new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

        public static string StudentDbInfoConnectionString
        {
            get
            {
                if (studentDbInfoConnectionString == null)
                {
                    var dbHost = Config["studentdbHost"];
                    var dbName = Config["studentdbName"];
                    var integratedSecurityFlag = "true";
                    var dbUserId = Config["studentdbUserId"];
                    var dbUserPassword = Config["studentdbUserPassword"];
                    var trustedConnection = "false";
                    studentDbInfoConnectionString = $"Server={dbHost};Database={dbName};Integrated Security={integratedSecurityFlag};User Id={dbUserId};Password={dbUserPassword};trusted_connection={trustedConnection};TrustServerCertificate=Yes;MultipleActiveResultSets=True";
                }

                return studentDbInfoConnectionString;
            }
        }


        //public static string FileAttachmentFilePath => Config["FileAttachmentFilePath"];
        //public static string DirectoryAttachmentPath => Config["DirectoryAttachmentPath"];
        //public static string studentDirectoryAttachmentPath => Config["studentDirectoryAttachmentPath"];


        public static string RootFilePath => Config["RootFilePath"];
        public static string TempFilePath => Config["TempFilePath"];
        public static string SpecificStudentFilePath => Config["SpecificStudentFilePath"];

    }
}