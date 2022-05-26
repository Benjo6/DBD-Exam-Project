
namespace lib.DTO
{
    public class GeoPointDto
    {
        public GeoPointDto(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
        public GeoPointDto()
        {

        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}