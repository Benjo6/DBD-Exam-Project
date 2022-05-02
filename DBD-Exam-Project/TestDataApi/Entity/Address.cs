using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class Address
    {
        public Address()
        {
            PersonalData = new HashSet<PersonalDatum>();
            Pharmacies = new HashSet<Pharmacy>();
        }

        public int Id { get; set; }
        public string Streetname { get; set; } = null!;
        public string Streetnumber { get; set; } = null!;
        public string Zipcode { get; set; } = null!;

        public virtual ICollection<PersonalDatum> PersonalData { get; set; }
        public virtual ICollection<Pharmacy> Pharmacies { get; set; }
    }
}
