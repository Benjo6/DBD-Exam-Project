using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.DTO
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public DateOnly? Expiration { get; set; }
        public DateTime Creation { get; set; }
        public MedicineDto Medicine { get; set; }
        public PatientDto Patient { get; set; }


    }

   

}
