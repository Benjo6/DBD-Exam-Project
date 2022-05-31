namespace DataAnalyzingService.Model
{
    public class ConsultationNDto
    {
        public string Id { get; set; }
        public DateTime? ConsultationStartUtc { get; set; }
        public DateTime CreatedUtc { get; set; }

        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Regarding { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
