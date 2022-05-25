using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("patient", Schema = "prescriptions")]
public class Patient: EntityWithId<int>
{
    public Patient(string cpr, PersonalDatum personalData, List<Prescription> prescriptions)
    {
        Cpr = cpr;
        PersonalData = personalData;
        Prescriptions = prescriptions;
    }

    [Column("id"), Key]
    public int Id { get; set; }
    [Column("cpr"), MaxLength(10)]
    public string Cpr { get; set; }
    [Column("personal_data_id")]
    public int PersonalDataId { get; set; }

    public virtual PersonalDatum PersonalData { get; set; }
    public virtual List<Prescription> Prescriptions { get; set; }
}