#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace lib.DTO;
public class PersonDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? CphNumber { get; set; }
    public string? PharmacyName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PersonType? Type { get; set; }
    public AddressDto? Address { get; set; }
}
