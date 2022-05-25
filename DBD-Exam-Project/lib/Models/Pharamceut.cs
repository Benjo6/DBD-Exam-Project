using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("pharmaceut", Schema = "prescriptions")]
public class Pharmaceut
{
    public Pharmaceut(PersonalDatum personalData, List<Prescription> prescriptions)
    {
        PersonalData = personalData;
        Prescriptions = prescriptions;
    }

    [Column("id"), Key]
    public int Id { get; set; }
    [Column("pharmacy_id")]
    public int? PharamacyId { get; set; }
    [Column("personal_data_id")]
    public int PersonalDataId { get; set; }

    public PersonalDatum PersonalData { get; set; }
    public Pharmacy? Pharmacy { get; set; }
    public List<Prescription> Prescriptions { get; set; }
}