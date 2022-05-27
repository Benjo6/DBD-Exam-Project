using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;

namespace lib.Models;

[Table("medicine", Schema = "prescriptions"), MessagePackObject(keyAsPropertyName: true)]
public class Medicine
{
    public Medicine(string name)
    {
        Name = name;
    }

    [Column("id"), System.ComponentModel.DataAnnotations.Key]
    public int Id { get; set; }
    [Column("name"), MaxLength(64)]
    public string Name { get; set; }

    [IgnoreMember]
    public List<Prescription> Prescriptions { get; set; } = new();
}