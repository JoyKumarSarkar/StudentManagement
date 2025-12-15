namespace StudentWepApi.Models
{
    public class CityResponse
    {
        public long CityId { get; set; }
        public string? CityName { get; set; }
        public long StateId { get; set; }
        public bool? IsActive { get; set; }

    }
}