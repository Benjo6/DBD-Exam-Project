using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public int PersonalDataId { get; set; }

        public virtual PersonalDatum PersonalData { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
