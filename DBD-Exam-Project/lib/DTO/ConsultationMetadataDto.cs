using System;

namespace lib.DTO
{
    public class ConsultationMetadataDto
    {
        public string Id { get; set; }
        public DateTime? ConsultationStartUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int CreatedCount { get; set; }

    }
}