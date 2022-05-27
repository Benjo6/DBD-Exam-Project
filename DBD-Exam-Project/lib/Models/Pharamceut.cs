using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("pharmaceut", Schema = "prescriptions")]
public class Pharmaceut : EntityWithId<int>
{
    [Column("id"), Key]
    public int Id { get; set; }
    [Column("pharmacy_id")]
    public int? PharamacyId { get; set; }
    [Column("personal_data_id")]
    public int PersonalDataId { get; set; }

    public PersonalDatum PersonalData { get; set; } = null!;
    public Pharmacy? Pharmacy { get; set; }
    public List<Prescription> Prescriptions { get; set; } = new();
}