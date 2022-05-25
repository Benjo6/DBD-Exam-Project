using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("medicine", Schema = "prescriptions")]
public class Medicine
{
    public Medicine(string name, List<Prescription> prescriptions)
    {
        Name = name;
        Prescriptions = prescriptions;
    }

    [Column("id"), Key]
    public int Id { get; set; }
    [Column("name"), MaxLength(64)]
    public string Name { get; set; }
        
    public List<Prescription> Prescriptions { get; set; }
}