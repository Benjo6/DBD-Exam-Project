namespace DataAnalyzer.Model
{
    public class PharmacyNDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
        public string? ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
