namespace DataAnalyzingService.Model
{
    public class PersonNDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CprNumber { get; set; }
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
        public string? ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? PharmacyName { get; set; }

    }
}
