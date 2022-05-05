using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StaffAdminApi;

[BsonIgnoreExtraElements]
public class Staff
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get;set; }
    public DateTime DateCreated { get; set; }

    public string? Name { get; set; }

}
