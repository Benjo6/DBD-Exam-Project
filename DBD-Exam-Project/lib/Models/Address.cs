using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models
{
    public partial class Address
    {

        public Address()
        {
            PersonalData = new HashSet<PersonalDatum>();
            Pharmacies = new HashSet<Pharmacy>();
        }

        public int Id { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }

        public virtual ICollection<PersonalDatum> PersonalData { get; set; }
        public virtual ICollection<Pharmacy> Pharmacies { get; set; }
    }
}
