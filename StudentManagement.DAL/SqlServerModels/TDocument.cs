using System.ComponentModel.DataAnnotations;

namespace STUDENTMANAGEMENT.DAL.SQLServerModels
{
    public class TDocument
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Size { get; set; }

        public long StudentId { get; set; }

        public string FileName { get; set; }

        public string OriginalFileName { get; set; }
    }
}
