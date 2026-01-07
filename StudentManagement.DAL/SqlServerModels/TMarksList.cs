using System.ComponentModel.DataAnnotations;

namespace STUDENTMANAGEMENT.DAL.SQLServerModels
{
    public class TMarksList
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }

        public decimal Marks { get; set; }
    }
}
