using System.ComponentModel.DataAnnotations;

namespace STUDENTMANAGEMENT.DAL.SQLServerModels
{
    public class TApplicationLog
    {
        [Key]
        public int ApplicationLogId { get; set; }
        public string LogDetails { get; set; }
        public int LogType { get; set; }
        public DateTime LogDate { get; set; }
        public string Root { get; set; }
        public int LogAssembly { get; set; }
    }
}
