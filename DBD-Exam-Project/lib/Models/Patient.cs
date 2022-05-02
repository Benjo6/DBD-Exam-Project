using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public string Cpr { get; set; } = null!;
        public int PersonalDataId { get; set; }

        public virtual PersonalDatum PersonalData { get; set; } = null!;
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
