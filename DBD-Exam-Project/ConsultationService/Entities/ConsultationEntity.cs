using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace ConsultationService.Entities;

[BsonIgnoreExtraElements]

public class ConsultationEntity
{

    [BsonIgnoreIfNull]
    public DateTime? ConsultationStartUtc { get; set; }
    [BsonIgnoreIfNull]
    public DateTime CreatedUtc { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ConsultationId { get; set; }

    [BsonIgnoreIfNull]
    public string? PatientId { get; set; }
    [BsonIgnoreIfNull]
    public string? DoctorId { get; set; }
    [BsonIgnoreIfNull]
    public string? Regarding { get; set; }
    [BsonIgnoreIfNull]
    public GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }
}
