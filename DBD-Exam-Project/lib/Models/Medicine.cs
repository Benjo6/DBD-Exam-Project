using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models
{
    public partial class Medicine
    {
        public Medicine()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
