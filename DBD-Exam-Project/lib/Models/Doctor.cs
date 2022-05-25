using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("doctor", Schema = "prescriptions")]
public class Doctor
{
    public Doctor(PersonalDatum personalData, List<Prescription> prescriptions)
    {
        PersonalData = personalData;
        Prescriptions = prescriptions;
    }

    [Key, Column("id")]  
    public int Id { get; set; }
    [Column("personal_data_id")]
    public int PersonalDataId { get; set; }

    public PersonalDatum PersonalData { get; set; }
    public List<Prescription> Prescriptions { get; set; }
}

