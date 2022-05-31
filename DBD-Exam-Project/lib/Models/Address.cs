using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;

namespace lib.Models;

[Table("address", Schema = "prescriptions"), MessagePackObject(keyAsPropertyName: true)]
public class Address
{
    [Column("id"), System.ComponentModel.DataAnnotations.Key]
    public int Id { get; set; }
    [Column("streetname"), MaxLength(64)]
    public string StreetName { get; set; }
    [Column("streetnumber"), MaxLength(8)]
    public string StreetNumber { get; set; }
    [Column("zipcode"), MaxLength(32)]
    public string ZipCode { get; set; }
    [Column("longitude")]
    public double Longitude { get; set; }
    [Column("latitude")]
    public double Latitude { get; set; }
    

    [IgnoreMember]
    public List<PersonalDatum> PersonalData { get; set; } = new();
    [IgnoreMember]
    public List<Pharmacy> Pharmacies { get; set; } = new();

    public Address(string streetName, string streetNumber, string zipCode)
    {
        StreetName = streetName;
        StreetNumber = streetNumber;
        ZipCode = zipCode;
    }
    public Address(string streetName, string streetNumber, string zipCode, double longitude, double latitude)
    {
        StreetName = streetName;
        StreetNumber = streetNumber;
        ZipCode = zipCode;
        Longitude = longitude;
        Latitude = latitude;
        }
}