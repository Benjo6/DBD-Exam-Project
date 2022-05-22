using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConsultationService.Entities;

[BsonIgnoreExtraElements]

public class ConsultationEntity
{

    [BsonIgnoreIfNullAttribute]
    public DateTime? ConsultationStartUtc { get; set; }
    [BsonIgnoreIfNullAttribute]
    public DateTime CreatedUtc { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ConsultationId { get; set; }

    [BsonIgnoreIfNullAttribute]
    public string? PatientId { get; set; }
    [BsonIgnoreIfNullAttribute]
    public string? DoctorId { get; set; }
    [BsonIgnoreIfNullAttribute]
    public string? Regarding { get; set; }
}
