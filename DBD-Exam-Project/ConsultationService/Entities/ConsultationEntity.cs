using MongoDB.Bson;

namespace ConsultationService.Entities;

public class ConsultationEntity : BsonDocument
{
    public DateTime ConsultationStartUtc { get; set; }
    public DateTime CreatedUtc { get; set; }

    public BsonObjectId Id { get; set; }

    public string PatientId { get; set; }
    public string DoctorId { get; set; }
    public string Regarding { get; set; }


}
