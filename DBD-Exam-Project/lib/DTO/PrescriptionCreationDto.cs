using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.DTO;
public class PrescriptionCreationDto
{
    [Required]
    public DateTime Expiration { get; set; }
    [Required]
    public string MedicineName { get; set; }
    [Required] 
    public string PatientCprNumber { get; set; }
    [Required]
    public int DoctorId { get; set; }
}
