using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JDataSupplier.Model
{
    public class PrescriptionNDto
    {
        public long Id { get; set; }
        public DateTime? Expiration { get; set; }
        public DateTime Creation { get; set; }
        public string MedicineName { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
