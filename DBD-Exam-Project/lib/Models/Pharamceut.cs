using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models
{
    public partial class Pharamceut
    {
        public Pharamceut()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public int? PharamacyId { get; set; }
        public int PersonalDataId { get; set; }

        public virtual PersonalDatum PersonalDatá { get; set; } = null!;
        public virtual Pharmacy? Pharmacy { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
