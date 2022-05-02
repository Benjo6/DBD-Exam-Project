using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models
{
    public partial class Pharmacy
    {
        public Pharmacy()
        {
            Pharamceuts = new HashSet<Pharamceut>();
        }

        public int Id { get; set; }
        public string PharmacyName { get; set; }
        public int? AddressId { get; set; }

        public virtual Address? Address { get; set; }
        public virtual  ICollection<Pharamceut> Pharamceuts { get; set; }
    }
}
