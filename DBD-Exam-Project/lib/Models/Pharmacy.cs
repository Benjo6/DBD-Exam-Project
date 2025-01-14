﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;

namespace lib.Models;

[Table("pharmacy", Schema = "prescriptions"), MessagePackObject(keyAsPropertyName: true)]
public class Pharmacy: EntityWithId<int>
{
    public Pharmacy(string pharmacyName)
    {
        PharmacyName = pharmacyName;
    }

    [Column("id"), System.ComponentModel.DataAnnotations.Key]
    public int Id { get; set; }
    [Column("pharmacy_name"), MaxLength(64)]
    public string PharmacyName { get; set; }
    [Column("address_id")]
    public int? AddressId { get; set; }

    public Address? Address { get; set; }
    [IgnoreMember]
    public List<Pharmaceut> Pharamceuts { get; set; } = new();
}