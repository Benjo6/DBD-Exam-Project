using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;

namespace lib.Models;

[Table("doctor", Schema = "prescriptions"), MessagePackObject(keyAsPropertyName: true)]
public class Doctor: EntityWithId<int>
{
    [System.ComponentModel.DataAnnotations.Key, Column("id")]  
    public int Id { get; set; }
    [Column("personal_data_id")]
    public int PersonalDataId { get; set; }

    public PersonalDatum PersonalData { get; set; } = null!;
    [IgnoreMember]
    public List<Prescription> Prescriptions { get; set; } = new();
}

