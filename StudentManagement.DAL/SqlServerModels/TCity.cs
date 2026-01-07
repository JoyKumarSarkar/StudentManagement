using System.ComponentModel.DataAnnotations;

namespace STUDENTMANAGEMENT.DAL.SQLServerModels
{
    public class TCity
    {
        [Key]
        public int Id { get; set; }
        public string LogDeNametails { get; set; }
        public int StateId { get; set; }
        public int IsActive { get; set; }
    }
}
