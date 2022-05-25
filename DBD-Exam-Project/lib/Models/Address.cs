using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("address", Schema = "prescriptions")]
public class Address
{
    [Column("id"), Key]
    public int Id { get; set; }
    [Column("streetname"), MaxLength(64)]
    public string StreetName { get; set; }
    [Column("streetnumber"), MaxLength(8)]
    public string StreetNumber { get; set; }
    [Column("zipcode"), MaxLength(32)]
    public string ZipCode { get; set; }

    public List<PersonalDatum> PersonalData { get; set; } = new();
    public List<Pharmacy> Pharmacies { get; set; } = new();

    public Address(string streetName, string streetNumber, string zipCode)
    {
        StreetName = streetName;
        StreetNumber = streetNumber;
        ZipCode = zipCode;
    }
}