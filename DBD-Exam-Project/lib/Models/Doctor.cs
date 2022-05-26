using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("doctor", Schema = "prescriptions")]
public class Doctor
{
    [Key, Column("id")]  
    public int Id { get; set; }
    [Column("personal_data_id")]
    public int PersonalDataId { get; set; }

    public PersonalDatum PersonalData { get; set; } = null!;
    public List<Prescription> Prescriptions { get; set; } = new();
}

