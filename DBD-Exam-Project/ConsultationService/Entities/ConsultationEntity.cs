using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConsultationService.Entities;

[BsonIgnoreExtraElements]
public class ConsultationEntity : BsonDocument
{
    public DateTime ConsultationStartUtc { get; set; }
    public DateTime CreatedUtc { get; set; }

    [BsonId]
    public BsonObjectId Id { get; set; }

    public string PatientId { get; set; }
    public string DoctorId { get; set; }
    public string Regarding { get; set; }
}
