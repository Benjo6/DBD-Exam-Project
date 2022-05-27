using System;

namespace lib.DTO
{
    public class ConsultationDto
    {
        public string Id { get; set; }
        public DateTime? ConsultationStartUtc { get; set; }
        public DateTime CreatedUtc { get; set; }

        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Regarding { get; set; }
        public GeoPointDto GeoPoint { get; set; }
    }
}