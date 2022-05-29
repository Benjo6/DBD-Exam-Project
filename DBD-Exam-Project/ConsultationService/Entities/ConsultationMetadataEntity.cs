using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConsultationService.Entities
{
    public class ConsultationMetadataEntity
    {
        [BsonIgnoreIfNull]
        public DateTime CreatedUtc { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonIgnoreIfNull]
        public DateTime DayOfConsultationsAdded { get; set; }
        public int ConsultationsCount { get; set; }
    }
}
