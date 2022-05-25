using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("pharmacy", Schema = "prescriptions")]
public class Pharmacy: EntityWithId<int>
{
    public Pharmacy(string pharmacyName, List<Pharmaceut> pharamceuts)
    {
        PharmacyName = pharmacyName;
        Pharamceuts = pharamceuts;
    }

    [Column("id"), Key]
    public int Id { get; set; }
    [Column("pharmacy_name"), MaxLength(64)]
    public string PharmacyName { get; set; }
    [Column("address_id")]
    public int? AddressId { get; set; }

    public Address? Address { get; set; }
    public List<Pharmaceut> Pharamceuts { get; set; }
}