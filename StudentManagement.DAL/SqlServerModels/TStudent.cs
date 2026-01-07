using System.ComponentModel.DataAnnotations;

namespace STUDENTMANAGEMENT.DAL.SQLServerModels
{
    public class TStudent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateOnly Dob { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string EmailId { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Code { get; set; }
        
        public int IsActive { get; set; }
    }
}
