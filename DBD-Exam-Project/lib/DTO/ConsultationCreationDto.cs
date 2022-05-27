using System;

namespace lib.DTO
{
    public class ConsultationCreationDto
    {
        public DateTime ConsultationStartUtc { get; set; }
        public GeoPointDto GeoPoint { get; set; }
        public string DoctorId { get; set; }
    }
}