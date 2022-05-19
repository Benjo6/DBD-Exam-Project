using System;

namespace lib.DTO
{
    public class ConsultationBookingRequestDto
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string Regarding { get; set; }
    }
}