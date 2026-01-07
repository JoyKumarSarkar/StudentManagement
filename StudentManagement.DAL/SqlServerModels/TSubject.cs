using System.ComponentModel.DataAnnotations;

namespace STUDENTMANAGEMENT.DAL.SQLServerModels
{
    public class TSubject
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int IsActive { get; set; }
    }
}
